using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerAttributes : MonoBehaviour, IDamageable
{
    internal static Action OnDeath;
    
    [Header("PlantMeter")]
    [SerializeField] internal float plantMeterStart = default, plantMeterMax = default;

    internal float plantMeterCurrent;
    internal float MovementSpeed { get; private set; } = 1f;
    internal BulletType BulletType { get; private set; }

    private UpgradeManager _upgradeManager;
    private int _level;

    private void OnEnable()
    {
        if (_upgradeManager == null) _upgradeManager = GetComponent<UpgradeManager>();

        UpgradeManager.OnLevelUp += ApplyUpgrade;

        plantMeterCurrent = plantMeterStart;
        
    }
    private void OnDisable()
    {
        UpgradeManager.OnLevelUp -= ApplyUpgrade;
    }

    private void Update()
    {
        if (Input.GetButton("Jump"))
        {
            AddToPlantMeter(Time.deltaTime);
        }
        else
        {
            AddToPlantMeter(-Time.deltaTime);
        }
    }

    private void ApplyUpgrade(Upgrade upgrade)
    {
        print(upgrade.UpgradeText);
        switch (upgrade.Type)
        {
            case UpgradeType.MovementSpeed:
                MovementSpeed += upgrade.FloatValue;
                break;

            case UpgradeType.BulletType:
                BulletType = upgrade.BulletTypeValue;
                break;
        }
    }

    internal void AddToPlantMeter(float plantEnergy)
    {
        plantMeterCurrent +=  plantEnergy;
        if(plantMeterCurrent >= plantMeterMax)
        {
            plantMeterCurrent = plantMeterStart + (plantMeterCurrent - plantMeterMax);
            _upgradeManager.LevelUp();
        }
    }

    public void TakeDamage(float incomingDamage)
    {
        plantMeterCurrent -= incomingDamage;

        if (plantMeterCurrent <= plantMeterStart)
        {
            OnDeath?.Invoke();
        }
    }
}
