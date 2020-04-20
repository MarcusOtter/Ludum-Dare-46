using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Plant : MonoBehaviour, IWaterable, IDamageable
{
    [Header("General plant settings")]
    [SerializeField] internal PlantType PlantType;
    [SerializeField] internal string Name;
    [SerializeField] internal string Description;
    [SerializeField] protected float ActiveRadius;

    [Header("Health")]
    [SerializeField] private float _fullyGrownMaxHealth = 20f;
    [SerializeField] private float _seedlingMaxHealth = 10f;
    [SerializeField] private float _seedlingStartHealth = 2f;

    [Header("Sprites")]
    [SerializeField] private Sprite _deadSprite;
    [SerializeField] private Sprite _seedlingSprite;
    [SerializeField] private Sprite _fullyGrownSprite;

    [Header("References")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private HealthBar _healthBar; 

    internal bool IsDead { get; private set; }
    protected float Efficiency { get; private set; }
    protected GrowthStage GrowthStage { get; private set; }

    private float _health;

    private void Awake()
    {
        GrowthStage = PlantType == PlantType.Sacred 
            ? GrowthStage.FullyGrown 
            : GrowthStage.Seedling;

        _health = PlantType == PlantType.Sacred 
            ? _fullyGrownMaxHealth 
            : _seedlingStartHealth;

        _healthBar.SetHealth(_health, GetCurrentMaxHealth());
    }

    protected virtual void Update()
    {
        // Placeholder 
    }

    public void Water(float waterAmount)
    {
        ModifyHealth(waterAmount);
    }

    public void Damage(float incomingDamage)
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
        return GrowthStage == GrowthStage.Seedling
            ? _seedlingMaxHealth
            : _fullyGrownMaxHealth;
    }

    private void ModifyHealth(float amount)
    {
        if (IsDead) { return; }

        _health += amount;

        _health = Mathf.Clamp(_health, 0f, GetCurrentMaxHealth());

        _healthBar.SetHealth(_health, GetCurrentMaxHealth());

        if (_health <= 0)
        {
            Die();
            return;
        }

        if (GrowthStage == GrowthStage.Seedling && _health >= _seedlingMaxHealth)
        {
            SetNewGrowthStage(GrowthStage.FullyGrown);
        }
    }

    private void Die()
    {
        IsDead = true;
        SetNewGrowthStage(GrowthStage.Dead);

        if (PlantType == PlantType.Sacred)
        {
            print("Game over");
        }
    }

    private void SetNewGrowthStage(GrowthStage growthStage)
    {
        // Remove this if we wanna be able to cure the dead plants
        if (IsDead) { return; }

        GrowthStage = growthStage;
        _spriteRenderer.sprite = GetSpriteForGrowthStage(growthStage);
        // Could do particle effects here and sounds, maybe animation for sacred idk
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
