using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour
{
    [SerializeField] private float _attackDelay = 1f;
    [SerializeField] private float _attackTime = 0.25f;
    [SerializeField] private InputActionReference _attackInput;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMovement _playerMovement;
    private bool _canAttack = true;

    void Start()
    {
        _attackInput.action.started += Attack;
    }

    void Update()
    {
        
    }

    void Attack(InputAction.CallbackContext context)
    {
        if (!_canAttack)
            return;

        _canAttack = false;
        _playerMovement.SetCanMoveState(false);
        _animator.SetTrigger("Attack");
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(_attackTime);
        _playerMovement.SetCanMoveState(true);
        yield return new WaitForSeconds(_attackDelay);
        _canAttack = true;
    }

    void OnDestroy()
    {
        _attackInput.action.started -= Attack;
    }
}
