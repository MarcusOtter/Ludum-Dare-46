using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunPlant : Plant
{
    [Header("Gun plant settings")]
    [SerializeField] private float _attackDelayBase = 2f;
    [SerializeField] private float _projectileDamageBase = 5f;
    [SerializeField] private float _projectileSpeed = 4f;
    [SerializeField] private Bullet _projectile;

    private float _lastBulletSpawnTime;
    private List<AmmoPlant> _plantsGivingBullets = new List<AmmoPlant>();

    private void OnEnable()
    {
        AmmoPlant.OnDeath += HandleAmmoPlantDeath;
    }

    internal void MarkBulletsAvailable(AmmoPlant sender)
    {
        _plantsGivingBullets.AddIfNotContains(sender);
    }

    protected override void Update()
    {
        base.Update();

        if (GrowthStage != GrowthStage.FullyGrown) { return; }

        if (_plantsGivingBullets.Count == 0) { return; }

        if (Time.time >= _lastBulletSpawnTime + GetCurrentAttackDelay())
        {
            ShootNearbyEnemies();
        }
    }

    private float GetCurrentAttackDelay()
    {
        return _attackDelayBase / GetEfficiencyFactor();
    }

    private float GetCurrentProjectileDamage()
    {
        return _projectileDamageBase * GetEfficiencyFactor();
    }

    private void HandleAmmoPlantDeath(AmmoPlant sender)
    {
        _plantsGivingBullets.RemoveIfContains(sender);
    }

    private void ShootNearbyEnemies()
    {
        var colliders = Physics2D.CircleCastAll(transform.position, ActiveRadius, Vector2.zero)
            .Select(x => x.collider)
            .Distinct()
            .ToArray();

        foreach (var collider in colliders)
        {
            if (!collider.TryGetComponent<Enemy>(out var enemy)) { continue; }
            if (enemy.CurrentState == EnemyState.Dead) { continue; }

            var bulletDirection = (Vector2) (enemy.transform.position - transform.position).normalized;

            var bullet = Instantiate(_projectile, transform.position, Quaternion.identity);
            bullet.transform.up = bulletDirection;
            bullet.Shoot(gameObject, GetCurrentProjectileDamage(), _projectileSpeed);

            _lastBulletSpawnTime = Time.time;
            return;
        }
    }

    private void OnDisable()
    {
        AmmoPlant.OnDeath -= HandleAmmoPlantDeath;
    }
}
