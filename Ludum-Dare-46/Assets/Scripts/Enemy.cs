using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDamageable, ICircleOnHover
{
    internal Action<EnemyState, EnemyState> OnStateChanged;

    [Header("General Enemy Settings")]
    [SerializeField] private float _maxHealth = 5f;
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _damage = 5f;
    [SerializeField] private float _attackDelay = 3f;
    [SerializeField] private float _attackRadius = 2f;
    [SerializeField] private float _waterStunDuration = 4f;

    [Header("References")]
    [SerializeField] private PlantDetector _plantDetector;
    [SerializeField] private HealthBar _healthBar;

    [Header("Audio settings")]
    [SerializeField] private SoundEffect _damagedSound;

    internal EnemyState CurrentState;
    internal Vector2 CurrentVelocity => _rigidbody.velocity;

    private Rigidbody2D _rigidbody;
    private Transform _sacredPlant;

    private Transform _target;

    private Vector2 _movementDirection;
    private List<WaterArea> _waterAreasToFleeFrom = new List<WaterArea>();
    private float _health;
    private float _lastAttackTime = -100f;
    private float _lastStunTime = -100f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _health = _maxHealth;
        _healthBar.SetHealth(_health, _maxHealth);
    }

    private void Start()
    {
        var sacredPlant = GameObject.FindGameObjectWithTag(EnvironmentVariables.SacredPlantTag);
        if (sacredPlant != null) { _sacredPlant = sacredPlant.transform; }
    }

    private void FixedUpdate()
    {
        if (CurrentState == EnemyState.Dead) 
        {
            _rigidbody.velocity = Vector2.zero;
            return; 
        }

        if (_waterAreasToFleeFrom.Count != 0)
        {
            Flee();
            return;
        }

        if (Time.time < _lastStunTime + _waterStunDuration)
        {
            SetState(EnemyState.Stunned);
            _rigidbody.velocity = Vector2.zero;
            return;
        }

        _target = GetClosestTarget();
        if (_target == null) { return; }

        var distanceToTarget = (_target.position - transform.position).magnitude;
        if (distanceToTarget <= _attackRadius)
        {
            SetState(EnemyState.Attacking);
            _rigidbody.velocity = Vector2.zero;

            var canAttack = Time.time >= _lastAttackTime + _attackDelay;
            if (!canAttack) { return; }

            Attack();
            _lastAttackTime = Time.time;
        }
        else
        {
            SetState(EnemyState.Hunting);
            _movementDirection = (_target.position - transform.position).normalized;
            Move();
        }
    }

    private void Flee()
    {
        SetState(EnemyState.Fleeing);
        _movementDirection = -(_waterAreasToFleeFrom[0].transform.position - transform.position).normalized;
        Move();
    }

    private void SetState(EnemyState state)
    {
        if (CurrentState == EnemyState.Dead) { return; }

        if (CurrentState != state)
        {
            OnStateChanged?.Invoke(CurrentState, state);
        }

        CurrentState = state;
    }

    private void Attack()
    {
        var colliders = Physics2D.CircleCastAll(transform.position, _attackRadius, Vector2.zero)
            .Select(x => x.collider)
            .Distinct()
            .ToArray();


        foreach(var collider in colliders)
        {
            if (!collider.TryGetComponent<Plant>(out var plant)) { continue; }
            plant.Damage(_damage);
        }
    }

    private Transform GetClosestTarget()
    {
        var plantsInRange = _plantDetector.GetPlantsInRange();

        if (plantsInRange.Length == 0) { return _sacredPlant; }

        return plantsInRange
            .OrderBy(x => (transform.position - x.transform.position).sqrMagnitude)
            .FirstOrDefault()
            .transform;
    }

    private void Move()
    {
        if (_target == null) { return; }

        _rigidbody.velocity = _movementDirection * _movementSpeed;
    }

    public void Damage(float incomingDamage)
    {
        if (CurrentState == EnemyState.Dead) { return; }

        _health -= incomingDamage;
        _healthBar.SetHealth(_health, _maxHealth);

        AudioManager.Instance.PlaySoundEffect(_damagedSound);

        if (_health <= 0) { Die(); }
    }

    private void Die()
    {
        SetState(EnemyState.Dead);
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.simulated = false;
        _healthBar.SetVisibility(false);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.TryGetComponent<WaterArea>(out var waterArea)) { return; }

        _waterAreasToFleeFrom.AddIfNotContains(waterArea);
        _movementDirection = -(waterArea.transform.position - transform.position).normalized;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.TryGetComponent<WaterArea>(out var waterArea)) { return; }

        _waterAreasToFleeFrom.RemoveIfContains(waterArea);

        if (_waterAreasToFleeFrom.Count == 0) 
        { 
            _lastStunTime = Time.time;
            SetState(EnemyState.Stunned);
        }
    }

    public float GetRadius()
    {
        return _plantDetector.DetectionRadius;
    }

    public Color GetColour()
    {
        return new Color32(255, 0, 0, 75);
    }
}
