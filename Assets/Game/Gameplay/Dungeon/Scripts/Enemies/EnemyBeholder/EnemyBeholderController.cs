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
    //[SerializeField] float _moveSpeed = 3f;
    //[SerializeField] float _runSpeed = 6f;
    [SerializeField] float _idleTimeout = 3f;
    [SerializeField] private Transform _player;
    [SerializeField] Collider _hitAttackCollider;
    [SerializeField] Collider _shockAttackCollider;
    [SerializeField] int[] _attackDamageAmount;
    public Vector3 HomePosition { get; private set; }
    public int[] AvailableAttacks => _attackDamageAmount;

    public float IdleTimeout => _idleTimeout;
    public bool IsAttacking { get; private set; }

    

    protected override void Awake()
    {
        base.Awake();
        HomePosition = transform.position;
        SetState(new EnemyBeholderIdleState(this, _animation, _navigation));
    }

    protected override void Start()
    {
        base.Start();
        DisableAttack();
        if (_player == null) _player = GameObject.FindWithTag("Player").transform;
    }


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
