using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Plant : MonoBehaviour, IWaterable, IDamageable
{
    [Header("General plant settings")]
    [SerializeField] internal PlantType PlantType;
    [SerializeField] internal string Name;
    [SerializeField] internal string Description;

    [Header("Health")]
    [SerializeField] protected float FullyGrownMaxHealth = 20f;
    [SerializeField] private float _seedlingMaxHealth = 5f;
    [SerializeField] private float _seedlingStartHealth = 1f;

    [Header("Sprites")]
    [SerializeField] private Sprite _deadSprite;
    [SerializeField] private Sprite _seedlingSprite;
    [SerializeField] private Sprite _fullyGrownSprite;

    [Header("References")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private HealthBar _healthBar; 

    protected float Efficiency { get; private set; }

    private GrowthStage _growthStage;
    private float _health;
    private bool _isDead;

    private void Awake()
    {
        _growthStage = GrowthStage.Seedling;
        _health = _seedlingStartHealth;
        _healthBar.SetHealth(_health, GetCurrentMaxHealth());
    }

    public void Water(float waterAmount)
    {
        ModifyHealth(waterAmount);
    }

    public void TakeDamage(float incomingDamage)
    {
        ModifyHealth(-incomingDamage);
    }

    internal void ModifyEfficiency(Plant efficiencyPlant, float efficiencyDelta)
    {
        if (efficiencyPlant == this) { return; }
        Efficiency += efficiencyDelta;
    }
    private float GetCurrentMaxHealth()
    {
        return _growthStage == GrowthStage.Seedling
            ? _seedlingMaxHealth
            : FullyGrownMaxHealth;
    }

    private void ModifyHealth(float amount)
    {
        if (_isDead) { return; }

        _health += amount;
        _healthBar.SetHealth(_health, GetCurrentMaxHealth());

        if (_health == 0)
        {
            _isDead = true;
            SetNewGrowthStage(GrowthStage.Dead); 
            return;
        }

        if (_growthStage == GrowthStage.Seedling && _health >= _seedlingMaxHealth)
        {
            SetNewGrowthStage(GrowthStage.FullyGrown);
        }
    }

    private void SetNewGrowthStage(GrowthStage growthStage)
    {
        // Remove this if we wanna be able to cure the dead plants
        if (_isDead) { return; }

        _growthStage = growthStage;
        _spriteRenderer.sprite = GetSpriteForGrowthStage(growthStage);
        // Could do particle effects here and sounds
    }

    private Sprite GetSpriteForGrowthStage(GrowthStage growthStage) 
    {
        switch (growthStage)
        {
            case GrowthStage.Dead:       return _deadSprite;
            case GrowthStage.Seedling:   return _seedlingSprite;
            case GrowthStage.FullyGrown: return _fullyGrownSprite;
            default:                     return null;
        }
    }
}
