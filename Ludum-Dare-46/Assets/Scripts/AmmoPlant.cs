using System;
using System.Linq;
using UnityEngine;

public class AmmoPlant : Plant
{
    internal static Action<AmmoPlant> OnDeath; 

    protected override void Update()
    {
        base.Update();

        if (GrowthStage == GrowthStage.Dead || GrowthStage != GrowthStage.FullyGrown) { return; }
        ProvideBulletsToGunPlants();
    }

    protected override void Die()
    {
        base.Die();
        OnDeath?.Invoke(this);
    }

    private void ProvideBulletsToGunPlants()
    {
        var colliders = Physics2D.CircleCastAll(transform.position, ActiveRadius * GetEfficiencyFactor(), Vector2.zero)
            .Select(x => x.collider)
            .Distinct()
            .ToArray();

        foreach (var collider in colliders)
        {
            if (!collider.TryGetComponent<GunPlant>(out var gunPlant)) { continue; }
            gunPlant.MarkBulletsAvailable(this);
            return;
        }
    }
}
