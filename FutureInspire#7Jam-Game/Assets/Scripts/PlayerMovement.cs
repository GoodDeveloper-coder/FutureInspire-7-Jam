using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _jumpForce = 5f;

    [Header("Check Ground")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _checkGroundPos;
    [SerializeField] private float _checkGroundRadius = 1.5f;

    [Header("Inputs")]
    [SerializeField] private InputActionReference _moveDirection;
    [SerializeField] private InputActionReference _jumpInput;

    [Space]
    [SerializeField] private Animator _animator;
    private Rigidbody _rb;
    private bool _canMove = true;
    private bool _isOnGround = false;
    private bool _canJump = true;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _jumpInput.action.started += Jump;
    }

    void Update()
    {
        Move();
        CheckGround();
    }

    void Move()
    {
        if (!_canMove)
            return;

        Vector2 moveInput = _moveDirection.action.ReadValue<Vector2>();
        Vector3 moveDirection = transform.TransformDirection(new Vector3(moveInput.x, 0, moveInput.y)) * _speed;
        moveDirection.y = _rb.velocity.y;
        _rb.velocity = moveDirection;

        _animator.SetFloat("Move", moveInput.sqrMagnitude);
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (!_canJump || !_canMove)
            return;
        
        _canJump = false;
        _animator.SetTrigger("Jump");
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    void CheckGround()
    {
        bool lastIsOnGround = _isOnGround;
        _isOnGround = Physics.CheckSphere(_checkGroundPos.position, _checkGroundRadius, _groundLayer);
        _animator.SetBool("IsOnGround", _isOnGround);

        if (!lastIsOnGround && _isOnGround)
            _canJump = true;
    }

    public void SetCanMoveState(bool canMove)
    {
        _canMove = canMove;

        if (!_canMove)
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
            _animator.SetFloat("Move", 0f);
        }
    }

    void OnDestroy()
    {
        _jumpInput.action.started -= Jump;   
    }
}
