using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Plant : MonoBehaviour, IWaterable, IDamageable
{
    [Header("General plant settings")]
    [SerializeField] private PlantType _plantType;

    [SerializeField] private float _seedlingMaxHealth;
    [SerializeField] private float _seedlingStartHealth;

    private GrowthStage _growthStage;
    private float _health;

    private void Awake()
    {
        _growthStage = GrowthStage.Seedling;
        _health = _seedlingStartHealth;
    }

    private void ModifyHealth(float amount)
    {
        _health += amount;

        if (_health == 0)
        {
            // Die
            return;
        }

        if (_growthStage == GrowthStage.Seedling && _health >= _seedlingMaxHealth)
        {
            // Upgrade
        }
    }

    public void HitByWaterBehaviour(float waterAmount)
    {
        _health += waterAmount;
    }

    public void TakeDamage(float incomingDamage)
    {
        _health -= incomingDamage;
    }
}
