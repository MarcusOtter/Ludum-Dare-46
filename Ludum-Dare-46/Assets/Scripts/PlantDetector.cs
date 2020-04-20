using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlantDetector : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private float _detectionRadius = 4f;

    private List<Plant> _plantsInRange = new List<Plant>();
    private CircleCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        _enemy.OnStateChanged += HandleEnemyStateChanged;
    }

    private void Update()
    {
        if (_collider.radius != _detectionRadius)
        {
            _collider.radius = _detectionRadius;
        }
    }

    private void HandleEnemyStateChanged(EnemyState prevState, EnemyState newState)
    {
        if (newState == EnemyState.Stunned)
        {
            _plantsInRange.Clear();
            _collider.enabled = false;
        }

        if (prevState == EnemyState.Stunned && newState != EnemyState.Stunned)
        {
            _collider.enabled = true;
        }
    }

    internal Plant[] GetPlantsInRange()
    {
        return _plantsInRange.Where(x => x.GrowthStage != GrowthStage.Dead).ToArray();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.TryGetComponent<Plant>(out var plant)) { return; }
        if (collider.CompareTag(EnvironmentVariables.SacredPlantTag)) { return; }
        _plantsInRange.AddIfNotContains(plant);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.TryGetComponent<Plant>(out var plant)) { return; }

        _plantsInRange.RemoveIfContains(plant);
    }

    private void OnDisable()
    {
        _enemy.OnStateChanged -= HandleEnemyStateChanged;
    }
}
