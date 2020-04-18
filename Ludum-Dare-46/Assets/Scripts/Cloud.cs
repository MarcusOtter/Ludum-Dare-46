using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Cloud : MonoBehaviour
{
    internal static Vector2 _movementDirection = new Vector2(3, 1);

    [SerializeField] [Range(0f, 5f)] private float _velocity;
    [SerializeField] [Range(-20f, 20f)] private float _angularVelocity;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = _movementDirection.normalized * _velocity;
        _rigidbody.angularVelocity = _angularVelocity;
    }
}
