using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerWeapon : MonoBehaviour
{
    internal static event Action<PlayerWeapon> OnWeaponFire;

    [Header("Player weapon settings")]
    [SerializeField] internal float RecoilKnockbackForce = 10;

    [Header("Water settings")]
    [SerializeField] private float _waterConsumptionPerSecond = 2f;
    [SerializeField] private int _numberOfPoints = 10;
    [SerializeField] private float _arcHeight = 0.3f;
    [SerializeField] private float _minRange = 0f;
    [SerializeField] private float _maxRange = 7f;
    [SerializeField] private WaterArea _splashPrefab;

    internal Vector2 AimDirection { get; private set; }

    private PlayerInput _userInput;
    private LineRenderer _waterRenderer;
    private Transform _weaponPivot;

    private WaterArea _spawnedSplash;

    private bool _attackBeingHeld;
    private bool _canShoot = true;

    private void OnEnable()
    {
        _userInput = PlayerInput.Instance;
        _weaponPivot = transform.parent;
        _waterRenderer = GetComponent<LineRenderer>();

        _userInput.OnAttackKeyDown += RegisterAttackKeyDown;
        _userInput.OnAttackKeyUp += RegisterAttackKeyUp;
    }

    private void Update()
    {
        RotateTowardsMouse();

        if (!_canShoot || !_attackBeingHeld || InventoryItem.hoveringBox) 
        {
            _waterRenderer.enabled = false;
            return; 
        }

        // If you don't have enough water, return

        _waterRenderer.enabled = true;
        SprayWater();

        // Deplete water with water consumption per second * Time.deltaTime
    }

    private void SprayWater()
    {
        _waterRenderer.positionCount = _numberOfPoints;

        var startPoint = transform.position;
        var endPoint = _userInput.MouseWorldPosition.With(z: 0);

        var distance = Vector3.Magnitude(endPoint - startPoint);
        var scaledArcHeight = _arcHeight * distance;

        if (distance <= _minRange) 
        {
            _waterRenderer.enabled = false;
            return;
        }
        else if (distance >= _maxRange)
        {
            endPoint = startPoint + Vector3.ClampMagnitude(endPoint - startPoint, _maxRange);

            // Recalculate arc scale
            distance = Vector3.Magnitude(endPoint - startPoint);
            scaledArcHeight = _arcHeight * distance;
        }

        var middlePoint = Vector3.Lerp(startPoint, endPoint, 0.5f).Add(y: scaledArcHeight);

        for (int i = 0; i < _numberOfPoints; i++)
        {
            float t = i / (float) (_numberOfPoints - 1);
            Vector3 position = (1 - t) * (1 - t) * startPoint + 2 * (1 - t) * t * middlePoint + t * t * endPoint;
            _waterRenderer.SetPosition(i, position);
        }

        if (_spawnedSplash != null)
        {
            _spawnedSplash.transform.position = endPoint;
        }
    }

    private void RotateTowardsMouse()
    {
        AimDirection = (_userInput.MouseWorldPosition - _weaponPivot.position).normalized;
        _weaponPivot.up = AimDirection;
    }

    private void RegisterAttackKeyDown(PlayerInput sender)
    {
        _attackBeingHeld = true;
        if (_spawnedSplash != null)
        {
            Destroy(_spawnedSplash.gameObject);
        }
        _spawnedSplash = Instantiate(_splashPrefab, sender.MouseWorldPosition, Quaternion.identity);
        _spawnedSplash.SetWaterConsumptionPerSecond(_waterConsumptionPerSecond);
    }

    private void RegisterAttackKeyUp(PlayerInput sender)
    {
        _attackBeingHeld = false;
        Destroy(_spawnedSplash.gameObject);
    }

    private void OnDisable()
    {
        _userInput.OnAttackKeyDown -= RegisterAttackKeyDown;
        _userInput.OnAttackKeyUp -= RegisterAttackKeyUp;
    }
}

