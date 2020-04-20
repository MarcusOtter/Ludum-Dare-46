using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Plant : MonoBehaviour, IWaterable, IDamageable, ICircleOnHover
{
    [Header("General plant settings")]
    [SerializeField] internal PlantType PlantType;
    [SerializeField] internal string Name;
    [SerializeField] internal string Description;
    [SerializeField] protected float ActiveRadius = 3f;

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

    internal GrowthStage GrowthStage { get; private set; }

    private float _health;

    private readonly List<HealthPlant> _healthBoostingPlants = new List<HealthPlant>();
    private readonly List<EfficiencyPlant> _efficiencyBoostingPlants = new List<EfficiencyPlant>();

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

    private void OnEnable()
    {
        HealthPlant.OnDeath += HandleHealthPlantDeath;
        EfficiencyPlant.OnDeath += HandleEfficiencyPlantDeath;
    }

    public void Water(float waterAmount)
    {
        ModifyHealth(waterAmount);
    }

    public void Damage(float incomingDamage)
    {
        ModifyHealth(-incomingDamage);
    }

    internal void AddEfficiencyBoost(EfficiencyPlant sender)
    {
        _efficiencyBoostingPlants.AddIfNotContains(sender);
        _healthBar.SetHealth(_health, GetCurrentMaxHealth());
    }

    internal void AddHealthBoost(HealthPlant sender)
    {
        _healthBoostingPlants.AddIfNotContains(sender);
        _healthBar.SetHealth(_health, GetCurrentMaxHealth());
    }

    protected float GetEfficiencyFactor()
    {
        var efficiencyBoost = _efficiencyBoostingPlants.Sum(x => x.EfficiencyIncrease);
        return efficiencyBoost == 0 
            ? 1 
            : 1 + efficiencyBoost;
    }

    protected virtual void Update()
    {
        // Placeholder
    }

    private void HandleEfficiencyPlantDeath(EfficiencyPlant sender)
    {
        _efficiencyBoostingPlants.RemoveIfContains(sender);
    }

    private void HandleHealthPlantDeath(HealthPlant sender)
    {
        _healthBoostingPlants.RemoveIfContains(sender);
    }

    private float GetCurrentMaxHealth()
    {
        var maxHealthIncrease = _healthBoostingPlants.Sum(x => x.GetHealthIncrease());

        var maxHealth = GrowthStage == GrowthStage.Seedling
            ? _seedlingMaxHealth
            : _fullyGrownMaxHealth;

        return maxHealthIncrease == 0
            ? maxHealth
            : maxHealth * (1 + maxHealthIncrease);
    }

    private void ModifyHealth(float amount)
    {
        if (GrowthStage == GrowthStage.Dead) { return; }

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

    protected virtual void Die()
    {
        SetNewGrowthStage(GrowthStage.Dead);
        _healthBar.SetVisibility(false);

        if (PlantType == PlantType.Sacred)
        {
            print("Game over");
        }
    }

    private void SetNewGrowthStage(GrowthStage growthStage)
    {
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

    private void OnDisable()
    {
        HealthPlant.OnDeath -= HandleHealthPlantDeath;
        EfficiencyPlant.OnDeath -= HandleEfficiencyPlantDeath;
    }

    public float GetRadius()
    {
        return ActiveRadius;
    }

    public Color GetColour()
    {
        return new Color32(0, 255, 0, 75);
    }
}
