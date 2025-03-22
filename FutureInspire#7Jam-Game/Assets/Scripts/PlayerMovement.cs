using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;

    [Space]
    [SerializeField] private InputActionReference _moveDirection;
    [SerializeField] private Animator _animator;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 moveInput = _moveDirection.action.ReadValue<Vector2>();
        Vector3 moveDirection = transform.TransformDirection(new Vector3(moveInput.x, 0, moveInput.y)) * _speed;
        moveDirection.y = _rb.velocity.y;
        _rb.velocity = moveDirection;

        _animator.SetFloat("Move", moveInput.sqrMagnitude);
    }
}
