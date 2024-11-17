using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWatcherController : Enemy
{
    [SerializeField] float _detectPlayerRange = 10f;
    [SerializeField] float _attackRange = 2f;
    [SerializeField] float _idleDuration = 3f;
    [SerializeField] float _moveSpeed = 1.5f;
    [SerializeField] float _runSpeed = 2.5f;
    [SerializeField] List<Transform> _patrolPoints;
    public float MoveSpeed => _moveSpeed;
    public float RunSpeed => _runSpeed;

    private int _currentPatrolIndex = 0;
    [SerializeField] float _idleTimeout = 3f;
    [SerializeField] Collider _hitAttackCollider;
    [SerializeField] int[] _attackDamageAmount;

    public int[] AvailableAttacks => _attackDamageAmount;
    public float IdleTimeout => _idleTimeout;
    public bool IsAttacking { get; private set; }

    Transform _player;


    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    protected override void Start()
    {
        base.Start();
        DisableAttack();
        SetState(new EnemyWatcherIdleState(this, _idleDuration));
    }

    public bool HasSufficientPatrolPoints() => _patrolPoints.Count > 1 && _patrolPoints != null;

    public bool IsPlayerClose() => Vector3.Distance(transform.position, _player.position) < _detectPlayerRange;
    public bool IsPlayerInAttackRange() => Vector3.Distance(transform.position, _player.position) <= _attackRange;

    public bool IsNearPosition(Vector3 targetPosition) => Vector3.Distance(transform.position, targetPosition) <= 0.2f;
    public Transform GetNextPatrolPoint()
    {
        if (!HasSufficientPatrolPoints())
        {
            Debug.LogWarning("No hay suficientes puntos de patrulla disponibles.");
            return transform;
        }
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, _patrolPoints.Count);
        } while (randomIndex == _currentPatrolIndex);

        _currentPatrolIndex = randomIndex;
        return _patrolPoints[_currentPatrolIndex];
    }

    public Vector3 MoveTowardsNexttPatrolPoint()
    {
        Vector3 position = GetNextPatrolPoint().position;
        _agent.destination = position;
        _agent.speed = _moveSpeed;
        return position;
    }

    public void LootAtPlayer()
    {
        LookAtTarget(_player.position);
    }

    void LookAtTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void MoveTowardsPlayer()
    {
        _agent.destination = _player.position;
        _agent.speed = _moveSpeed;
    }


    public Transform GetPlayer() => _player;

    public void EnableAttack(int attackNumber)
    {
        _currentDamageAmount = _attackDamageAmount[attackNumber - 1];
        _hitAttackCollider.enabled = true;
        IsAttacking = true;
    }

    public void DisableAttack()
    {
        _hitAttackCollider.enabled = false;
        IsAttacking = false;
    }
}
