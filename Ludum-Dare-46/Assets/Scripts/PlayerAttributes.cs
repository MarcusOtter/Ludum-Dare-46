using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(UpgradeManager))]
public class PlayerAttributes : MonoBehaviour, IDamageable
{
    internal static Action OnDeath;

    public float MovementSpeed;

    [Header("Plant meter")]
    [SerializeField] internal float plantMeterStart = 5f;
    [SerializeField] internal float plantMeterMax = 15f;
    [SerializeField] private float plantMeterChargeRate = 1f, plantMeterDepletionRate = 1f;

    internal float plantMeterCurrent;
    internal BulletType BulletType { get; private set; }

    private UpgradeManager _upgradeManager;
    private int _level;

    private int cloudCount = 0;


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

    private void FixedUpdate()
    {
        if(cloudCount < 1)
        {
            ChangePlantMeter(plantMeterChargeRate * Time.deltaTime);
        }
        else
        {
            ChangePlantMeter(-plantMeterDepletionRate * Time.deltaTime);
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
        }
    }

    internal void ChangePlantMeter(float plantEnergyDelta)
    {
        plantMeterCurrent +=  plantEnergyDelta;
        if(plantEnergyDelta > 0f)
        {
            //do cool effect somewhere else
            if(plantMeterCurrent >= plantMeterMax)
            {
                plantMeterCurrent = plantMeterStart + (plantMeterCurrent - plantMeterMax);
                _level++;
                _upgradeManager.LevelUp(_level);
            }
        }
        else
        {
            //do bad stuff somewhere else
            if(plantMeterCurrent <= 0f)
            {
                //print("Dead");
                plantMeterCurrent = 0f;
                OnDeath?.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Cloud>() != null)
        {
            cloudCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Cloud>() != null)
        {
            cloudCount--;
        }
    }


    public void TakeDamage(float incomingDamage)
    {
        ChangePlantMeter(-incomingDamage);
    }
}
