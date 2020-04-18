using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [Header("Level")]
    private int _level;

    [Header("PlantMeter")]
    [SerializeField] internal float plantMeterStart = default, plantMeterMax = default;
    internal float plantMeterCurrent;
    internal float MovementSpeed { get; private set; } = 1f;
    internal BulletType BulletType { get; private set; }

    private UpgradeManager _upgradeManager;


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

    private void ApplyUpgrade(Upgrade upgrade)
    {
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
}
