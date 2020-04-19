using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Plant : MonoBehaviour
{
    [Header("Plant meter")]
    [SerializeField] private float _plantMeterChargeRate = 0.05f;
    [SerializeField] private float _plantMeterDepletionRate = 0.1f;

    [Header("Plant parts")]
    [SerializeField] private float _singlePlantPieceSize = 2f;
    [SerializeField] private Transform _middlePlantPiecesParent;
    [SerializeField] private Transform _topPlantPiece;
    [SerializeField] private SpriteRenderer _middlePlantPiecePrefab;

    private BoxCollider2D _collider;
    private PlayerAttributes _playerAttributes;

    private int _cloudCount;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        UpdateColliderSize();
    }

    private void OnEnable()
    {
        PlayerAttributes.OnLevelUp += AddPlantPiece;
    }

    private void AddPlantPiece(int level, Upgrade newUpgrade)
    {
        if (newUpgrade.PlantUpgradeSprite == null)
        {
            Debug.LogError($"The upgrade \"{newUpgrade.name}\" does not contain a plant upgrade sprite.");
            return;
        }

        var newPlantPosition = _topPlantPiece.position;

        var newPlantPartRenderer = Instantiate(_middlePlantPiecePrefab, newPlantPosition, Quaternion.identity, _middlePlantPiecesParent);
        newPlantPartRenderer.sprite = newUpgrade.PlantUpgradeSprite;

        _topPlantPiece.position = _topPlantPiece.position.Add(y: _singlePlantPieceSize);
        UpdateColliderSize();
    }

    private void UpdateColliderSize()
    {
        var totalPlantPieces = _middlePlantPiecesParent.childCount + 2; // Top and bottom

        _collider.offset = _collider.offset.With(y: (totalPlantPieces * _singlePlantPieceSize) / 2);
        _collider.size = _collider.size.With(y: totalPlantPieces * _singlePlantPieceSize - 2f);
    }

    private void Start()
    {
        _playerAttributes = FindObjectOfType<PlayerAttributes>();
    }

    private void FixedUpdate()
    {
        if (_playerAttributes == null) { return; }

        var plantMeterDelta = _cloudCount < 1 ? _plantMeterChargeRate : -_plantMeterDepletionRate;
        _playerAttributes.ChangePlantMeter(plantMeterDelta * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Cloud>() != null)
        {
            _cloudCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<Cloud>() != null)
        {
            _cloudCount--;
        }
    }

    private void OnDisable()
    {
        PlayerAttributes.OnLevelUp -= AddPlantPiece;
    }
}
