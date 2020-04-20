using System;
using System.Linq;
using UnityEngine;

public class EfficiencyPlant : Plant
{
    internal static Action<EfficiencyPlant> OnDeath;

    [Header("Efficiency plant settings")]
    [SerializeField] internal float EfficiencyIncrease = 0.1f;

    protected override void Update()
    {
        base.Update();

        if (GrowthStage == GrowthStage.Dead || GrowthStage != GrowthStage.FullyGrown) { return; }
        BoostNearbyPlants();
    }

    protected override void Die()
    {
        base.Die();
        OnDeath?.Invoke(this);
    }

    private void BoostNearbyPlants()
    {
        var colliders = Physics2D.CircleCastAll(transform.position, ActiveRadius, Vector2.zero)
            .Select(x => x.collider)
            .Distinct()
            .ToArray();

        foreach (var collider in colliders)
        {
            if (!collider.TryGetComponent<Plant>(out var plant)) { continue; }
            if (plant.GrowthStage == GrowthStage.Dead) { continue; }
            if (plant.PlantType == PlantType.Efficiency) { continue; }
            if (collider.gameObject == gameObject) { continue; }

            plant.AddEfficiencyBoost(this);
        }
    }
}
