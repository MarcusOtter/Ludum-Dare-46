using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Cloud : MonoBehaviour
{
    internal static Vector2 _movementDirection;

    [SerializeField] private float _velocity;
    [SerializeField] private float _angularVelocity;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = _movementDirection * _velocity;
        _rigidbody.angularVelocity = _angularVelocity;
    }
}
