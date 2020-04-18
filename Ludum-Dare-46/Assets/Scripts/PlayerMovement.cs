﻿using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 1f;

    private PlayerInput _input;
    private Rigidbody2D _rigidbody;

    private PlayerAttributes _attributes;

    private Vector2 _recoilKnockbackVector;

    private void OnEnable()
    {
        PlayerWeapon.OnWeaponFire += AddRecoilKnockback;
    }

    private void Start()
    {
        _attributes = GetComponent<PlayerAttributes>();
        _input = PlayerInput.Instance;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity =
            Vector2.ClampMagnitude(new Vector2(_input.HorizontalAxis, _input.VerticalAxis), 1)
            * _attributes.MovementSpeed + _recoilKnockbackVector;

        // Reset recoil knockback vector after applying it
        _recoilKnockbackVector = Vector2.zero;
    }

    private void AddRecoilKnockback(object sender, EventArgs args)
    {
        var weapon = (PlayerWeapon)sender;
        var knockbackDirection = weapon.AimDirection * -1;

        _recoilKnockbackVector = knockbackDirection * weapon.RecoilKnockbackForce;
    }

    private void OnDisable()
    {
        PlayerWeapon.OnWeaponFire -= AddRecoilKnockback;
    }
}
