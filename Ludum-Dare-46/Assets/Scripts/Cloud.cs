using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Cloud : MonoBehaviour
{
    [SerializeField] private float _minVelocity = 0f;
    [SerializeField] private float _maxVelocity = 5f;


    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        var velocity = Random.Range(_minVelocity, _maxVelocity);


        var direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0;
        _rigidbody.angularDrag = 0;
        _rigidbody.drag = 0;
        _rigidbody.velocity = direction * velocity;
    }
}
