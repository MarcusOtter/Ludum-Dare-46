using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerWeapon : MonoBehaviour
{
    internal static event EventHandler OnWeaponFire;

    public Vector2 AimDirection { get; private set; }

    [Header("Bullet settings")]
    [SerializeField] protected Bullet BulletPrefabToSpawn;
    [SerializeField] protected int BulletDamage = 2;
    [SerializeField] protected float BulletSpeed = 40;

    [Header("Weapon settings")]
    [SerializeField] protected float ShootDelay = 0.15f;
    [SerializeField] [Range(0f, 1f)] protected float Accuracy = 0.925f;

    [Header("Audio settings")]
    [SerializeField] protected SoundEffect ShootSound;


    [Header("Player weapon settings")]
    [SerializeField] internal Transform ShellCasingSpawnPoint;
    [SerializeField] internal float RecoilKnockbackForce = 10;

    private Transform _parentTransform;

    private PlayerInput _userInput;

    private int _bulletsFiredThisHold; // How many bullets that have been fired during this mouse press

    private float _holdStartTime;
    private bool _attackBeingHeld;

    private float _lastBulletSpawnTime;

    private bool _fireWhenPossible;

    private bool _canShoot = true;

    private void OnEnable()
    {
        _userInput = PlayerInput.Instance;

        _parentTransform = transform.parent;
        _userInput.OnAttackKeyDown += RegisterAttackKeyDown;
        _userInput.OnAttackKeyUp += RegisterAttackKeyUp;
    }

    private void Update()
    {
        RotateTowardsMouse();

        if (!_canShoot) { return; }

        // Spawn a bullet if there is one that is queued to be spawned
        if (_fireWhenPossible && _lastBulletSpawnTime + ShootDelay < Time.time)
        {
            SpawnBullet();
            _fireWhenPossible = false;
        }

        if (!_attackBeingHeld) { return; }

        SprayBullets();
    }

    /// <summary>
    /// Spawns a bullet when the attack key is being held .
    /// </summary>
    private void SprayBullets()
    {
        if (Time.time - (_holdStartTime + _bulletsFiredThisHold * ShootDelay) > ShootDelay)
        {
            SpawnBullet();
            _bulletsFiredThisHold++;
        }
    }

    private void RotateTowardsMouse()
    {
        AimDirection = (_userInput.MouseWorldPosition - _parentTransform.position).normalized;
        _parentTransform.up = AimDirection;
    }

    private void RegisterAttackKeyDown(PlayerInput sender)
    {
        _holdStartTime = Time.time;
        _attackBeingHeld = true;

        if (_lastBulletSpawnTime + ShootDelay > Time.time)
        {
            // The player has pressed the attack key prematurely. 
            // This bullet will automatically be spawned when it can be.
            _fireWhenPossible = true;
        }
        else
        {
            SpawnBullet();
        }
    }

    private void RegisterAttackKeyUp(PlayerInput sender)
    {
        _attackBeingHeld = false;
        _bulletsFiredThisHold = 0;
    }

    private void SpawnBullet()
    {
        if (!_canShoot) { return; }

        OnWeaponFire?.Invoke(this, EventArgs.Empty);

        AudioManager.Instance.PlaySoundEffect(ShootSound);

        Instantiate(BulletPrefabToSpawn, transform.position, GetRandomOffsetBulletRotation()).Shoot(BulletDamage, BulletSpeed);
        _lastBulletSpawnTime = Time.time;
    }

    // Enable or disable shooting (called by unity events in tutorial)
    //public void EnableShooting(bool enable)
    //{
    //    if (_canShoot == enable) { return; }

    //    _canShoot = enable;

    //    if (!_canShoot)
    //    {
    //        _fireWhenPossible = false;
    //        RegisterAttackKeyUp(this);
    //    }
    //}

    /// <summary>
    /// Calculates a rotation with a random offset that depends on the <see cref="Weapon.Accuracy"/>.
    /// </summary>
    private Quaternion GetRandomOffsetBulletRotation()
    {
        // If the accuracy is 1, the bullet will always go in a straight line.
        if (Mathf.Approximately(Accuracy, 1f))
        {
            return transform.rotation;
        }

        // With 0 accuracy, the offset can be between -45 degrees and + 45 degrees.
        // With 0.5 accuracy, the offset can be between -22.5 degrees and + 22.5 degrees.
        // (etc)
        var randomOffsetDegrees = Random.Range((1 - Accuracy) * -45, (1 - Accuracy) * 45);
        return Quaternion.AngleAxis(randomOffsetDegrees, Vector3.forward) * transform.rotation;
    }

    private void OnDisable()
    {
        _userInput.OnAttackKeyDown -= RegisterAttackKeyDown;
        _userInput.OnAttackKeyUp -= RegisterAttackKeyUp;
    }
}
