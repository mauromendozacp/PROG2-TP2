using UnityEngine;
using UnityEngine.AI;

public class EnemyChestController : MonoBehaviour
{
    [SerializeField] float _detectItemRange = 4f;
    [SerializeField] float _detectPlayerRange = 6f;
    [SerializeField] float _idleDistance = 10f;
    [SerializeField] float _attackRange = 1f;
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] float _runSpeed = 4f;

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
}
