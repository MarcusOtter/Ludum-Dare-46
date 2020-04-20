using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _enemies;
    [SerializeField] private Transform[] _spawnpoints;
    [SerializeField] private float _spawnInterval;
    private float _lastSpawn = -10000;

    [SerializeField] private float _spawnStartTime;
    

    private void Start()
    {
        _spawnStartTime += Time.time;
    }

    private void Update()
    {
        if(Time.time > _lastSpawn + _spawnInterval)
        {
            SpawnEnemy(Random.Range(0, _enemies.Length),Random.Range(0, _spawnpoints.Length));
        }
    }

    private void SpawnEnemy(int enemyIndex, int locationIndex)
    {
        if (_enemies.Length == 0 || _spawnpoints.Length == 0 || Time.time < _spawnStartTime) return;
        Instantiate(_enemies[enemyIndex], _spawnpoints[locationIndex]);
        _lastSpawn = Time.time;
    }
}
