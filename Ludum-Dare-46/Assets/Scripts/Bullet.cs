using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet settings")]
    [SerializeField] private float _timeUntilDestroyed = 5f;

    [Header("Impact settings")]
    [SerializeField] private SoundEffect _impactSound;
    [SerializeField] private ParticleSystem _impactParicleSystem;
    [SerializeField] private ParticleSystem.MinMaxGradient _wallImpactColor;
    [SerializeField] private ParticleSystem.MinMaxGradient _enemyImpactColor;

    private float _damage;
    private Rigidbody2D _rigidbody;
    private GameObject _sender;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    internal void Shoot(GameObject sender, float damage, float speed)
    {
        _sender = sender;
        _damage = damage;
        _rigidbody.AddForce(transform.up * speed, ForceMode2D.Impulse);
        Destroy(transform.root.gameObject, _timeUntilDestroyed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleEnemyBulletCollision(collision.collider);
    }

    private void HandleEnemyBulletCollision(Collider2D collider)
    {
        if (_sender == null || collider.gameObject == _sender) { return; }
        if (collider.TryGetComponent<Bullet>(out _)) { return; }

        _rigidbody.velocity = Vector2.zero;

        var hitDamageable = collider.GetComponentInChildren<IDamageable>();

        if (hitDamageable != null)
        {
            hitDamageable.Damage(_damage);
        }
        else
        {
            // Play default impact sound if no IDamageable is hit
            AudioManager.Instance.PlaySoundEffect(_impactSound);
        }

        bool spawnBloodParticles = collider.GetComponent<Enemy>() != null ||
                                collider.CompareTag(EnvironmentVariables.PlayerTag);

        SpawnImpactParticles(spawnBloodParticles);

        Destroy(gameObject);
    }

    private void SpawnImpactParticles(bool spawnBloodParticles)
    {
        if (_impactParicleSystem == null) { return; }

        var particleSystem = Instantiate(_impactParicleSystem, transform.position, Quaternion.identity);
        var mainModule = particleSystem.main;

        particleSystem.transform.up = -transform.up;

        mainModule.startColor = spawnBloodParticles
            ? _enemyImpactColor
            : _wallImpactColor;
    }
}

