using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cactus : Plant
{
    [Header("Cactus settings")]
    [SerializeField] private float _attackDelay = 2f;
    [SerializeField] private float _projectileDamage = 5f;
    [SerializeField] private float _projectileSpeed = 2f;
    [SerializeField] private Bullet _projectile;
    [SerializeField] private float[] _anglesToShoot = new float[4] { 0f, 90f, 180f, 270f };

    private float _lastBulletSpawnTime;
    private List<Bullet> _spawnedBullets = new List<Bullet>();

    protected override void Update()
    {
        base.Update();

        if (GrowthStage != GrowthStage.FullyGrown) { return; }

        DestroyBulletsOutsideRadius();

        if (Time.time >= _lastBulletSpawnTime + _attackDelay) 
        {
            SpawnBullets();
            _lastBulletSpawnTime = Time.time;
        }
    }

    private void SpawnBullets()
    {
        foreach(var angle in _anglesToShoot)
        {
            var bullet = Instantiate(_projectile, transform.position, Quaternion.Euler(0, 0, angle));

            bullet.Shoot(gameObject, _projectileDamage, _projectileSpeed);
            _spawnedBullets.Add(bullet);
        }
    }

    private void DestroyBulletsOutsideRadius()
    {
        _spawnedBullets = _spawnedBullets.Where(x => x != null).ToList();

        foreach (var bullet in _spawnedBullets)
        {
            var bulletPosition = bullet.transform.position;

            if (Mathf.Abs(bulletPosition.x - transform.position.x) > ActiveRadius ||
                Mathf.Abs(bulletPosition.y - transform.position.y) > ActiveRadius)
            {
                Destroy(bullet.gameObject);
            }
        }
    }
}
