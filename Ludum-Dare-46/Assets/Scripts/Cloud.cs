using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Cloud : MonoBehaviour
{
    internal static Vector2 MovementDirection = new Vector2(3, 1);

    [SerializeField] private float _minVelocity = 0f;
    [SerializeField] private float _maxVelocity = 5f;

    [SerializeField] private float _minAngularVelocity = -20f;
    [SerializeField] private float _maxAngularVelocity = 20f;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        var velocity = Random.Range(_minVelocity, _maxVelocity);
        var angularVelocity = Random.Range(_minAngularVelocity, _maxAngularVelocity);

        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0;
        _rigidbody.angularDrag = 0;
        _rigidbody.drag = 0;
        _rigidbody.velocity = MovementDirection.normalized * velocity;
        _rigidbody.angularVelocity = angularVelocity;
    }
}
