using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(UpgradeManager))]
public class PlayerAttributes : MonoBehaviour, IDamageable
{
    internal static Action OnDeath;
    internal static Action<int, Upgrade> OnLevelUp;

    public float MovementSpeed;

    [Header("Plant meter")]
    [SerializeField] internal float PlantMeterStart = 0.25f;
    [SerializeField] internal float PlantMeterMax = 1f;

    internal float PlantMeterCurrent { get; private set; }

    private UpgradeManager _upgradeManager;

    private int _level;
    private bool _isDead;

    private void Awake()
    {
        _upgradeManager = GetComponent<UpgradeManager>();
        PlantMeterCurrent = PlantMeterStart;
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
            LevelUp();
        }

        if (PlantMeterCurrent <= 0f)
        {
            Die();
        }
    }

    private void LevelUp()
    {
        PlantMeterCurrent = PlantMeterStart + (PlantMeterCurrent - PlantMeterMax);

        _level++;
        var upgrade = _upgradeManager.GetUpgrade(_level);
        OnLevelUp?.Invoke(_level, upgrade);
        ApplyUpgrade(upgrade);
    }

    private void Die()
    {
        print("Dead");
        PlantMeterCurrent = 0f;
        OnDeath?.Invoke();
    }

    public void TakeDamage(float incomingDamage)
    {
        ChangePlantMeter(-incomingDamage);
    }
}
