using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 8f;
    [SerializeField] private float _runSpeed = 13f;
    [SerializeField] private float _jumpForce = 5f;

    [Header("Check Ground")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _checkGroundPos;
    [SerializeField] private float _checkGroundRadius = 1.5f;

    [Header("Inputs")]
    [SerializeField] private InputActionReference _moveDirection;
    [SerializeField] private InputActionReference _runInput;
    [SerializeField] private InputActionReference _jumpInput;

    [Space]
    [SerializeField] private Animator _animator;
    public bool _canMove { get; private set; }
    public bool _isOnGround { get; private set; }
    public float _energy { get; private set; }

    private bool _canJump = true;
    private bool _isRunning = false;
    private float _currentSpeed;
    private Rigidbody _rb;

    public event Action _startRunning;
    public event Action _stopRunning;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _jumpInput.action.started += Jump;
        _runInput.action.started += StartRun;
        _runInput.action.canceled += StopRun;
        _currentSpeed = _walkSpeed;
        _energy = 10f;
    }

    void Update()
    {
        Move();
        CheckGround();
        UpdateEnergy();
    }

    void Move()
    {
        if (!_canMove)
            return;

        Vector2 moveInput = _moveDirection.action.ReadValue<Vector2>();
        Vector3 moveDirection = transform.TransformDirection(new Vector3(moveInput.x, 0, moveInput.y)) * _currentSpeed;
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

    void StartRun(InputAction.CallbackContext context)
    {
        if (_energy <= 0)
            return;

        _isRunning = true;
        _currentSpeed = _runSpeed;
        _animator.SetBool("Run", true);
        _startRunning?.Invoke();
    }

    void StopRun(InputAction.CallbackContext context)
    {
        if (!_isRunning)
            return;

        _currentSpeed = _walkSpeed;
        _isRunning = false;
        _animator.SetBool("Run", false);
        _stopRunning.Invoke();
    }

    void UpdateEnergy()
    {
        if (_isRunning)
        {
            _energy = Math.Clamp(_energy - Time.deltaTime, 0, 10f);
            if (_energy <= 0)
            {
                StopRun(new InputAction.CallbackContext());
            }
        }
        else
        {
            _energy = Math.Clamp(_energy + Time.deltaTime, 0, 10f);
        }
    }

    void CheckGround()
    {
        bool lastIsOnGround = _isOnGround;
        _isOnGround = Physics.CheckSphere(_checkGroundPos.position, _checkGroundRadius, _groundLayer);
        _animator.SetBool("IsOnGround", _isOnGround);

        if (!lastIsOnGround && _isOnGround)
        {
            _canJump = true;
            StartCoroutine(DisableMovementForTime(1.0f));
            Camera.main.DOShakeRotation(1f, 0.5f);
        }
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

    private IEnumerator DisableMovementForTime(float time)
    {
        SetCanMoveState(false);
        yield return new WaitForSeconds(time);
        SetCanMoveState(true);
    }

    void OnDestroy()
    {
        _jumpInput.action.started -= Jump;
        _runInput.action.started -= StartRun;
        _runInput.action.canceled -= StopRun;   
    }
}
