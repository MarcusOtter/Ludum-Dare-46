using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class EnemyGraphics : MonoBehaviour
{
    [SerializeField] private string _walkingParameterName = "Walking";
    [SerializeField] private string _attackingParameterName = "Attacking";
    [SerializeField] private string _stunnedParameterName = "Stunned";

    private int _walkingParameterHash;
    private int _attackParameterHash;
    private int _stunnedParameterHash;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Enemy _enemy;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _walkingParameterHash = Animator.StringToHash(_walkingParameterName);
        _attackParameterHash = Animator.StringToHash(_attackingParameterName);
        _stunnedParameterHash = Animator.StringToHash(_stunnedParameterName);
    }

    private void OnEnable()
    {
        _enemy = GetComponentInParent<Enemy>();
        _enemy.OnStateChanged += HandleEnemyStateChanged;
    }

    private void Update()
    {
        var currentVelocityX = _enemy.CurrentVelocity.x;

        if (currentVelocityX != 0)
        {
            _spriteRenderer.flipX = currentVelocityX > 0.1f;
        }

        _animator.SetBool(_walkingParameterHash, Mathf.Abs(currentVelocityX) > 0.1f);
    }

    private void HandleEnemyStateChanged(EnemyState prevState, EnemyState newState)
    {
        switch (newState)
        {
            case EnemyState.Attacking:
                _animator.SetBool(_attackParameterHash, true);
                break;

            case EnemyState.Stunned:
                _animator.SetBool(_stunnedParameterHash, true);
                break;
                
            default:
                _animator.SetBool(_attackParameterHash, false);
                _animator.SetBool(_stunnedParameterHash, false);
                break;
        }
    }

    private void OnDisable()
    {
        _enemy.OnStateChanged -= HandleEnemyStateChanged;
    }
}
