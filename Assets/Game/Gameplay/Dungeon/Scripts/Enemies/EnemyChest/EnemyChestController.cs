using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChestController : Enemy
{
    [SerializeField] float _detectItemRange = 4f;
    [SerializeField] float _detectPlayerRange = 6f;
    [SerializeField] float _idleDistance = 10f;
    [SerializeField] float _attackRange = 2f;
    [SerializeField] float _idleTimeout = 3f;
    [SerializeField] Collider _hitAttackCollider;
    [SerializeField] int[] _attackDamageAmount;
    public int[] AvailableAttacks => _attackDamageAmount;
    public float IdleTimeout => _idleTimeout;
    public bool IsAttacking { get; private set; }
    [SerializeField] Transform _itemToProtect;

    public Transform ItemToProtect => _itemToProtect;

    protected override void Awake()
    {
        base.Awake();
        if (_itemToProtect == null) _itemToProtect = gameObject.transform;
    }

    protected override void Start()
    {
        base.Start();
        DisableAttack();
        SetState(new EnemyChestIdleState(this, _animation, _navigation));
    }

    public bool IsPlayerClose() => Vector3.Distance(transform.position, _player.position) < _detectPlayerRange;

    public bool IsPlayerFar() => Vector3.Distance(transform.position, _player.position) > _idleDistance;

    public bool IsNearItemToProtect() => Vector3.Distance(transform.position, _itemToProtect.position) < _detectItemRange;

    public bool IsPlayerInAttackRange() => Vector3.Distance(transform.position, _player.position) <= _attackRange;

    
    public void LookAtCollectible()
    {
        LookAtTarget(_itemToProtect.position);
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
