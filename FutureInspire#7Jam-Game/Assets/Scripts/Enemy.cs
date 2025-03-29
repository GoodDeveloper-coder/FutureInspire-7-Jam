using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _damage = 5f;
    [SerializeField] private float _attackRadius = 5f;
    [SerializeField] private float _attackDelay = 2f;
    [SerializeField] private Transform _target;
    [SerializeField] private LookAtTarget _lookATarget;
    [SerializeField] private Animator _animator;
    private EnemiesSpawner _enemySpawner;
    private HealthManager _targetHealthManager;
    private NavMeshAgent _agent;
    private HealthManager _healthManager;
    private bool _canMove = true;
    private bool _canAttack = true;

    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _agent = GetComponent<NavMeshAgent>();

        _healthManager._onDie += ()=> { GameManager._instance.KilledEnemy(); Destroy(gameObject); };
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, _target.position) < _attackRadius)
        {
            StartCoroutine(StartAttackWithDelay(0.5f));
        }
        else
        {
            Move();
        }

    }

    void Move()
    {
        if (!_canMove)
            return;
        
        _agent.SetDestination(_target.position);
        _animator.SetFloat("Move", 1);
    }

    public void StartAttack()
    {
        if (!_canAttack || Vector3.Distance(transform.position, _target.position) >= _attackRadius)
            return;

        _canAttack = false;
        _canMove = false;
        _animator.SetTrigger("Attack");
        _animator.SetFloat("Move", 0);
    }

    public void Attack()
    {
        if (Vector3.Distance(transform.position, _target.position) > _attackRadius)
        {
            StartCoroutine(AttackDelay());
            return;
        }

        if (_targetHealthManager == null)
            _targetHealthManager = _target.GetComponent<HealthManager>();

        _targetHealthManager.TakeDamage(_damage);
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(_attackDelay);
        _canAttack = true;
        _canMove = true;
    }

    private IEnumerator StartAttackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartAttack();
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _lookATarget.SetTarget(target);
    }

    void OnDestroy()
    {
        _enemySpawner.RemoveEnemy(gameObject);
        _healthManager._onDie -= ()=> { GameManager._instance.KilledEnemy(); Destroy(gameObject); };
    }

    public void SetSpawner(EnemiesSpawner enemiesSpawner)
    {
        _enemySpawner = enemiesSpawner;
    }
}
