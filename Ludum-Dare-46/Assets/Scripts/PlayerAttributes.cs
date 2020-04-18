using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(UpgradeManager))]
public class PlayerAttributes : MonoBehaviour, IDamageable
{
    internal static Action OnDeath;

    public float MovementSpeed;

    [Header("Plant meter")]
    [SerializeField] internal float PlantMeterStart = 0.25f;
    [SerializeField] internal float PlantMeterMax = 1f;
    [SerializeField] private float _plantMeterChargeRate = 0.05f, _plantMeterDepletionRate = 0.1f;

    internal float PlantMeterCurrent { get; private set; }

    private UpgradeManager _upgradeManager;

    private int _level;
    private int _cloudCount;
    private bool _isDead;

    private void Awake()
    {
        _upgradeManager = GetComponent<UpgradeManager>();
        PlantMeterCurrent = PlantMeterStart;
    }

    private void OnEnable()
    {
        UpgradeManager.OnLevelUp += ApplyUpgrade;
    }

    private void OnDisable()
    {
        UpgradeManager.OnLevelUp -= ApplyUpgrade;
    }

    private void FixedUpdate()
    {
        if (_isDead) { return; }

        var plantMeterDelta = _cloudCount < 1 ? _plantMeterChargeRate : -_plantMeterDepletionRate;
        ChangePlantMeter(plantMeterDelta * Time.fixedDeltaTime);
    }

    private void ApplyUpgrade(Upgrade upgrade)
    {
        if (_isDead) { return; }

        print(upgrade.UpgradeText);
        switch (upgrade.Type)
        {
            case UpgradeType.MovementSpeed:
                MovementSpeed += upgrade.FloatValue;
                break;
        }
    }

    internal void ChangePlantMeter(float plantEnergyDelta)
    {
        if (_isDead) { return; }

        PlantMeterCurrent += plantEnergyDelta;

        if (PlantMeterCurrent >= PlantMeterMax)
        {
            PlantMeterCurrent = PlantMeterStart + (PlantMeterCurrent - PlantMeterMax);
            _level++;
            _upgradeManager.LevelUp(_level);
        }

        if (PlantMeterCurrent <= 0f)
        {
            print("Dead");
            PlantMeterCurrent = 0f;
            OnDeath?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<Cloud>() != null)
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

    public void TakeDamage(float incomingDamage)
    {
        ChangePlantMeter(-incomingDamage);
    }
}
