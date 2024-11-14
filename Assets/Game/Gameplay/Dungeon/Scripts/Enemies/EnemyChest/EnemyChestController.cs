using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChestController : MonoBehaviour
{
    [SerializeField] float _detectItemRange = 4f;
    [SerializeField] float _detectPlayerRange = 6f;
    [SerializeField] float _idleDistance = 10f;
    [SerializeField] float _attackRange = 2f;
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] float _runSpeed = 4f;
    [SerializeField] private LayerMask _playerLayer;
    //[SerializeField] int _biteDamageAmount = 2;
    //[SerializeField] int _hitDamageAmount = 5;
    [SerializeField] float _idleTimeout = 3f;
    [SerializeField] Collider _hitAttackCollider;
    [SerializeField] LayerMask _targetLayer;
    [SerializeField] int[] _attackDamageAmount;
    public int[] AvailableAttacks => _attackDamageAmount;
    public float IdleTimeout => _idleTimeout;
    public bool IsAttacking { get; private set; }
    int _currentDamageAmount = 0;
    IEnemyState _currentState;
    [SerializeField] Transform _collectible;

    Transform _player;
    Animator _anim;
    NavMeshAgent _agent;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
        DisableAttack();
        SetState(new EnemyChestIdleState(this));
    }

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

    public bool IsPlayerClose() => Vector3.Distance(transform.position, _player.position) < _detectPlayerRange;

    public bool IsPlayerFar() => Vector3.Distance(transform.position, _player.position) > _idleDistance;

    public bool IsNearCollectible() => Vector3.Distance(transform.position, _collectible.position) < _detectItemRange;

    public bool IsPlayerInAttackRange() => Vector3.Distance(transform.position, _player.position) <= _attackRange;

    
    public void LookAtCollectible()
    {
        LookAtTarget(_collectible.position);
    }

    public void LootAtPlayer()
    {
        LookAtTarget(_player.position);
    }

    public void MoveTowardsPlayer()
    {
        MoveTowards(_player.position, _moveSpeed);
    }

    public void RunTowardsPlayer()
    {
        //MoveTowards(_player.position, _runSpeed);
        _agent.destination = _player.position;
    }

    public void MoveTowardsCollectible()
    {
        //MoveTowards(_collectible.position, _moveSpeed);
        _agent.destination = _collectible.position;
    }

    void LookAtTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetAgentDestination(Vector3 destination)
    {
        _agent.destination = destination;
    }


    public void ResetAgentDestination()
    {
        _agent.ResetPath();
    }

    private void Attack(int damageAmount)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, _attackRange, _playerLayer);
        Debug.Log($"Jugadores detectados: {hitEnemies.Length}");
        foreach (Collider collider in hitEnemies)
        {
            IDamagable damageable = collider.GetComponent<IDamagable>();
            if (damageable != null)
            {
                Debug.Log($"Aplicando daño a: {collider.name}");
                damageable.Damage(damageAmount);
            }
            else
            {
                Debug.LogWarning($"El objeto {collider.name} no implementa IDamagable.");
            }
        }
    }

    public void AttackBite()
    {
        //Attack(_biteDamageAmount);
    }

    public void AttackHit()
    {
        //Attack(_hitDamageAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckLayerInMask(_targetLayer, other.gameObject.layer))
        {
            IDamagable recieveDamage = other.gameObject.GetComponent<IDamagable>();
            recieveDamage?.Damage(_currentDamageAmount);
            Debug.Log($"Le hago daño de {_currentDamageAmount} a {other.name}");
        }
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
