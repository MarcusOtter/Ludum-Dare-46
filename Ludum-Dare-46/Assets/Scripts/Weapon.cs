using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Vector2 AimDirection { get; protected set; }

    [Header("Bullet settings")]
    [SerializeField] protected Bullet BulletPrefabToSpawn;
    [SerializeField] protected int BulletDamage = 2;
    [SerializeField] protected float BulletSpeed = 40;

    [Header("Weapon settings")]
    [SerializeField] protected float ShootDelay = 0.15f;
    [SerializeField] [Range(0f, 1f)] protected float Accuracy = 0.925f;

    [Header("Audio settings")]
    [SerializeField] protected SoundEffect ShootSound;


    public void ChangeBullet(Upgrade upgrade)
    {
        print("Hello");
        
        if (upgrade.Type != UpgradeType.Bullet) return;

        if (upgrade.BulletPrefab != null)
        {
            BulletPrefabToSpawn = upgrade.BulletPrefab;
        }
        else
            Debug.LogError($"The upgrade {upgrade.name} is of type bullet but has no bullet prefab attached");
    }

    protected virtual void Update()
    {
        WeaponBehaviour();
    }

    protected abstract void WeaponBehaviour();
}
