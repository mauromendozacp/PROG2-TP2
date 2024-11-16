using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWatcherController : Enemy
{
    [SerializeField] float _detectPlayerRange = 10f;
    [SerializeField] float _attackRange = 2f;
    [SerializeField] float _idleDuration = 3f;
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] float _runSpeed = 4f;
    [SerializeField] List<Transform> _patrolPoints;
    public float MoveSpeed => _moveSpeed;
    public float RunSpeed => _runSpeed;

    //private IEnemyState _currentState;
    private int _currentPatrolIndex = 0;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] float _idleTimeout = 3f;
    [SerializeField] Collider _hitAttackCollider;
    //[SerializeField] LayerMask _targetLayer;
    [SerializeField] int[] _attackDamageAmount;

    public int[] AvailableAttacks => _attackDamageAmount;
    public float IdleTimeout => _idleTimeout;
    public bool IsAttacking { get; private set; }
    //int _currentDamageAmount = 0;

    Transform _player;
    //Animator _anim;
    //NavMeshAgent _agent;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
        DisableAttack();
        SetState(new EnemyWatcherIdleState(this, _idleDuration));
    }

    /*
    private void Update()
    {
        _currentState.Execute();
    }

    public void SetState(IEnemyState newState)
    {
        _currentState = newState;
        _currentState.EnterState();
    }

    public void SetAnimator(string paramName, bool value)
    {
        _anim.SetBool(paramName, value);
    }

    public void TriggerAnimator(string name)
    {
        _anim.SetTrigger(name);
    }
    */

    public bool IsPlayerClose() => Vector3.Distance(transform.position, _player.position) < _detectPlayerRange;
    public bool IsPlayerInAttackRange() => Vector3.Distance(transform.position, _player.position) <= _attackRange;

    public bool IsNearPosition(Vector3 targetPosition) => Vector3.Distance(transform.position, targetPosition) <= 0.2f;
    public Transform GetNextPatrolPoint()
    {
        if (_patrolPoints.Count <= 1)
        {
            Debug.LogWarning("No hay suficientes puntos de patrulla disponibles.");
            return null;
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
        Debug.Log($"Proxima posición: {position}");
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
        MoveTowards(_player.position, _moveSpeed);
    }

    public void RunTowardsPlayer()
    {
        _agent.destination = _player.position;
    }


    public void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(target);
    }

    public Transform GetPlayer() => _player;

    /*
    public void SetAgentDestination(Vector3 destination)
    {
        _agent.destination = destination;
    }


    public void ResetAgentDestination()
    {
        _agent.ResetPath();
    }
    */

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckLayerInMask(_targetLayer, other.gameObject.layer))
        {
            IDamagable recieveDamage = other.gameObject.GetComponent<IDamagable>();
            recieveDamage?.Damage(_currentDamageAmount);
            Debug.Log($"Le hago daño de {_currentDamageAmount} a {other.name}");
        }
    }
    */

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
