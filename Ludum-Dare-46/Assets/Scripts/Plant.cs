using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Plant : MonoBehaviour, IWaterable, IDamageable
{
    [Header("General plant settings")]
    [SerializeField] internal PlantType PlantType;

    [SerializeField] private float _seedlingMaxHealth;
    [SerializeField] private float _seedlingStartHealth;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _deadSprite;
    [SerializeField] private Sprite _seedlingSprite;
    [SerializeField] private Sprite _fullyGrownSprite;

    private GrowthStage _growthStage;
    private float _health;
    private bool _isDead;

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

    private void SetNewGrowthStage(GrowthStage growthStage)
    {
        _growthStage = growthStage;

        switch (growthStage)
        {
            case GrowthStage.Dead:
                _isDead = true;
                _spriteRenderer.sprite = _deadSprite;
                break;

            case GrowthStage.Seedling:
                _spriteRenderer.sprite = _seedlingSprite;
                break;
        }
    }

    public void HitByWaterBehaviour(float waterAmount)
    {
        ModifyHealth(waterAmount);
    }

    public void TakeDamage(float incomingDamage)
    {
        ModifyHealth(-incomingDamage);
    }
}
