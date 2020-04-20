using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;

    private PlayerInput _input;
    private Rigidbody2D _rigidbody;

    private Vector2 _recoilKnockbackVector;

    private void OnEnable()
    {
        PlayerWeapon.OnWeaponFire += AddRecoilKnockback;
    }

    private void Start()
    {
        _input = PlayerInput.Instance;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity =
            Vector2.ClampMagnitude(new Vector2(_input.HorizontalAxis, _input.VerticalAxis), 1)
            * _movementSpeed + _recoilKnockbackVector;

        // Reset recoil knockback vector after applying it
        _recoilKnockbackVector = Vector2.zero;
    }

    private void AddRecoilKnockback(PlayerWeapon sender)
    {
        var knockbackDirection = sender.AimDirection * -1;
        _recoilKnockbackVector = knockbackDirection * sender.RecoilKnockbackForce;
    }

    private void OnDisable()
    {
        PlayerWeapon.OnWeaponFire -= AddRecoilKnockback;
    }
}
