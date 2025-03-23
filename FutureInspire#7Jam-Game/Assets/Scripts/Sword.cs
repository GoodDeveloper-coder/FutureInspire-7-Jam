using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Sword : MonoBehaviour
{
    [Header("Sword Settings")]
    [SerializeField] private float _attackDelay = 1f;
    [SerializeField] private float _attackTime = 0.25f;

    [Header("Colliders")]
    [SerializeField] private Transform _checkColliderPos;
    [SerializeField] private float _colliderRadius = 1f;

    [Header("Inputs")]
    [SerializeField] private InputActionReference _attackInput;

    [Header("Vfx")]
    [SerializeField] private ParticleSystem _slashVfx;

    [Space]
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
        if (!_canAttack || !_playerMovement._canMove || !_playerMovement._isOnGround)
            return;

        _canAttack = false;
        _playerMovement.SetCanMoveState(false);
        _animator.SetTrigger("Attack");
        Camera.main.DOShakeRotation(1f, 0.25f);

        Collider[] colliders = Physics.OverlapSphere(_checkColliderPos.position, _colliderRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag == "Enemy")
            {
                Destroy(collider.gameObject);
            }
        }

        StartCoroutine(StartSlashVfx(0.2f));
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(_attackTime);
        _playerMovement.SetCanMoveState(true);
        yield return new WaitForSeconds(_attackDelay);
        _canAttack = true;
    }

    private IEnumerator StartSlashVfx(float delay)
    {
        yield return new WaitForSeconds(delay);
        _slashVfx.Play();
    }

    void OnDestroy()
    {
        _attackInput.action.started -= Attack;
    }
}
