using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Cloud : MonoBehaviour
{
    [SerializeField] private float _maxWaterAmount = 20f;
    [SerializeField] private float _minVelocity = 0f;
    [SerializeField] private float _maxVelocity = 5f;

    private Rigidbody2D _rigidbody;

    private float _currentWaterAmount;
    private Vector3 _startingScale;

    private void Awake()
    {
        _startingScale = transform.localScale;
        _currentWaterAmount = _maxWaterAmount;

        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0;
        _rigidbody.angularDrag = 0;
        _rigidbody.drag = 0;

        SendInRandomDirection();
    }

    private void SendInRandomDirection()
    {
        var velocity = Random.Range(_minVelocity, _maxVelocity);
        var direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        _rigidbody.velocity = direction * velocity;
    }

    internal void ModifyWater(float waterDelta)
    {
        _currentWaterAmount += waterDelta;
        _currentWaterAmount = Mathf.Clamp(_currentWaterAmount, 0f, _maxWaterAmount);

        // Shrink cloud
        transform.localScale = _startingScale * Mathf.Clamp(_currentWaterAmount / _maxWaterAmount, 0.01f, _startingScale.x);
    }

    internal void ResetCloud()
    {
        transform.localScale = _startingScale;
        _currentWaterAmount = _maxWaterAmount;
    }
}
