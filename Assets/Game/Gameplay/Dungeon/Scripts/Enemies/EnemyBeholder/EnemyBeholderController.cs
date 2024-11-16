using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBeholderController : Enemy
{
    [SerializeField] float _startChaseDistance = 8f;
    [SerializeField] float _stopChaseDistance = 12f;
    [SerializeField] float _attackDistance = 2f;
    [SerializeField] float _walkSpeed = 3f;
    [SerializeField] float _runSpeed = 6f;
    [SerializeField] float _idleTimeout = 3f;
    [SerializeField] private Transform _player;
    //[SerializeField] IEnemyState _currentState;
    [SerializeField] Collider _hitAttackCollider;
    [SerializeField] Collider _shockAttackCollider;
    //[SerializeField] LayerMask _targetLayer;
    [SerializeField] int[] _attackDamageAmount;
    //Animator _anim;
    //NavMeshAgent _agent;
    //int _currentDamageAmount = 0;
    public Vector3 HomePosition { get; private set; }
    public int[] AvailableAttacks => _attackDamageAmount;

    public float IdleTimeout => _idleTimeout;
    public bool IsAttacking { get; private set; }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        HomePosition = transform.position;
        SetState(new EnemyBeholderIdleState(this));
    }

    private void Start()
    {
        DisableAttack();
        if (_player == null) _player = GameObject.FindWithTag("Player").transform;
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

    public void SetAnimator(string name, bool value)
    {
        _anim.SetBool(name, value);
    }

    public void TriggerAnimator(string name)
    {
        _anim.SetTrigger(name);
    }
    */

    public Transform GetPlayer() => _player;

    public bool IsPlayerClose() => Vector3.Distance(transform.position, _player.position) < _startChaseDistance;

    public bool IsPlayerFar() => Vector3.Distance(transform.position, _player.position) > _stopChaseDistance;
        
    public bool IsPlayerInAttackRange() => Vector3.Distance(transform.position, _player.position) <= _attackDistance;

    public bool IsAtHome() => Vector3.Distance(transform.position, HomePosition) < 0.2f;

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

    public void MoveTowardsPlayer()
    {
        _agent.destination = _player.position;
    }

    public void MoveTowardsHome()
    {
        _agent.destination = HomePosition;
    }

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
        if (attackNumber % 2 == 0) _shockAttackCollider.enabled = true;
        else _hitAttackCollider.enabled = true;
        IsAttacking = true;
    }

    public void DisableAttack()
    {
        _hitAttackCollider.enabled = false;
        _shockAttackCollider.enabled = false;
        IsAttacking = false;
    }
}
