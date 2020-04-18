//using UnityEngine;

//public class CloudSpawner : MonoBehaviour
//{
//    [SerializeField] private Transform[] _spawnPoints;
//    [SerializeField] private Cloud _cloudPrefab;

//    [SerializeField] private float _cloudDelay = 10f;
//    [SerializeField] private Vector2 _movementVector;
//    [SerializeField] private float _minRotationSpeed = -10f;
//    [SerializeField] private float _maxRotationSpeed = 10f;
    
//    private float _lastCloudSpawnTime;

//    private void FixedUpdate()
//    {
//        Move();
//        if (Time.time < _lastCloudSpawnTime + _cloudDelay ) { return; }

//        var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
//        var rotationSpeed = Random.Range(_minRotationSpeed, _maxRotationSpeed);
//        Instantiate(_cloudPrefab, spawnPoint.position, spawnPoint.rotation, transform).Initialize(rotationSpeed);

//        _lastCloudSpawnTime = Time.time;
//    }

//    private void Move()
//    {
//        transform.position += (Vector3) _movementVector;
//    }
//}
