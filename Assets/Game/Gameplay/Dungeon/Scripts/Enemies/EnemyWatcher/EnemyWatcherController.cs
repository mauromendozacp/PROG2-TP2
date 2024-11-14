using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWatcherController : MonoBehaviour
{
    [SerializeField] float _detectPlayerRange = 10f;
    [SerializeField] float _attackRange = 2f;
    [SerializeField] float _idleDuration = 3f;
    [SerializeField] float _patrolSpeed = 2f;
    [SerializeField] float _chaseSpeed = 4f;
    [SerializeField] List<Transform> _patrolPoints;
    public float PatrolSpeed => _patrolSpeed;
    public float ChaseSpeed => _chaseSpeed;

    private IEnemyState _currentState;
    private Animator _anim;
    private Transform _player;
    private int _currentPatrolIndex = 0;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
        SetState(new EnemyWatcherIdleState(this, _idleDuration));
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

    public void SetAnimator(string paramName, bool value)
    {
        _anim.SetBool(paramName, value);
    }

    public void TriggerAnimator(string name)
    {
        _anim.SetTrigger(name);
    }

    public bool IsPlayerClose() => Vector3.Distance(transform.position, _player.position) < _detectPlayerRange;
    public bool IsPlayerInAttackRange() => Vector3.Distance(transform.position, _player.position) <= _attackRange;
    public Transform GetNextPatrolPoint()
    {
        //Transform patrolPoint = _patrolPoints[_currentPatrolIndex];
        //_currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
        //return patrolPoint;
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

    public void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(target);
    }

    public Transform GetPlayer() => _player;
}
