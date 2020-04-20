using System;
using System.Linq;
using UnityEngine;

public class HealthPlant : Plant
{
    internal static Action<HealthPlant> OnDeath;

    [Header("Health plant settings")]
    [SerializeField] private float _healthIncreaseBase = 0.2f;

    internal float GetHealthIncrease()
    {
        return _healthIncreaseBase * GetEfficiencyFactor();
    }

    protected override void Update()
    {
        base.Update();

        if (GrowthStage == GrowthStage.Dead || GrowthStage != GrowthStage.FullyGrown) { return; }
        HealNearbyPlants();
    }

    protected override void Die()
    {
        base.Die();
        OnDeath?.Invoke(this);
    }

    private void HealNearbyPlants()
    {
        var colliders = Physics2D.CircleCastAll(transform.position, ActiveRadius, Vector2.zero)
            .Select(x => x.collider)
            .Distinct()
            .ToArray();

        foreach (var collider in colliders)
        {
            if (!collider.TryGetComponent<Plant>(out var plant)) { continue; }
            if (plant.GrowthStage == GrowthStage.Dead) { continue; }
            if (collider.gameObject == gameObject) { continue; }

            plant.AddHealthBoost(this);
        }
    }
}
