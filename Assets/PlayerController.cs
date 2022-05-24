using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float collisionOffset = 0.05f;
    [SerializeField] private ContactFilter2D movementFilter;
    
    private Vector2 _moveInput;
    private Rigidbody2D _rb2D;
    private Animator _animator;

    private List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();
    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_moveInput != Vector2.zero)
        {
            bool success = TryMove(_moveInput);

            if (!success)
            {
                success = TryMove(new Vector2(_moveInput.x, 0));
                if (!success)
                {
                    success = TryMove(new Vector2(0,_moveInput.y));
                }
            }
        }
        else
        {
            _animator.SetBool("isWalking",false);
        }
    }

    private bool TryMove(Vector2 direction)
    {
        int count = _rb2D.Cast(direction, movementFilter, _castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset);
        if (count == 0)
        {
            _animator.SetBool("isWalking",true);
            _rb2D.MovePosition(_rb2D.position + direction * (moveSpeed * Time.fixedDeltaTime));
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();

        if (_moveInput != Vector2.zero)
        {
            _animator.SetFloat("XInput", _moveInput.x);
            _animator.SetFloat("YInput", _moveInput.y);
        }
    }
}
