using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    NavMeshAgent _agent;
    Transform _player;
    float _moveSpeed, _runSpeed;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform;
        Enemy enemy = _agent.GetComponent<Enemy>();
        _moveSpeed = enemy.MoveSpeed;
        _runSpeed = enemy.RunSpeed;
    }

    public void SetAgentDestination(Vector3 destination)
    {
        _agent.destination = destination;
    }

    public void ResetAgentDestination()
    {
        _agent.ResetPath();
    }

    public void MoveTowardsPlayer()
    {
        _agent.destination = _player.position;
        _agent.speed = _runSpeed;
    }

    public void MoveTo(Vector3 position)
    {
        _agent.destination = position;
        _agent.speed = _moveSpeed;
    }
}
