using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("General Enemy Settings")]
    [SerializeField] protected float MaxHealth;
    [SerializeField] protected float MovementSpeed;

    [Header("Audio settings")]
    [SerializeField] private SoundEffect _damagedSound;

    [Header("Enemy events")]
    protected Action OnDeath;
    protected Action OnDamageTaken;

    protected bool IsDead;
    protected float Health;

    protected Rigidbody2D Rigidbody;
    protected Transform PlayerTransform;

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Health = MaxHealth;
    }

    protected virtual void OnEnable()
    {
        PlayerAttributes.OnDeath += OnPlayerDeathBehaviour;
    }

    protected virtual void Start()
    {
        var playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            PlayerTransform = playerMovement.transform.root;
        }
    }


    protected virtual void FixedUpdate()
    {
        if (IsDead) { return; }

        Move();
    }

    protected abstract void Move();

    public void TakeDamage(float incomingDamage)
    {
        Health -= incomingDamage;
        OnDamageTaken?.Invoke();
        PlayDamagedSound();

        if (Health <= 0) { Die(); }
    }

    private void PlayDamagedSound()
    {
        AudioManager.Instance.PlaySoundEffect(_damagedSound);
    }

    protected void Die()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }

    private void OnPlayerDeathBehaviour()
    {
        enabled = false;
    }

    protected virtual void OnDisable()
    {
        PlayerAttributes.OnDeath -= OnPlayerDeathBehaviour;
    }
}
