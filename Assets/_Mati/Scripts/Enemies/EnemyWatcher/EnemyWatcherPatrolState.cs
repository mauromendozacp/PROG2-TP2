using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWatcherPatrolState : IEnemyState
{
    private EnemyWatcherController _controller;
    private Transform _patrolTarget;

    public EnemyWatcherPatrolState(EnemyWatcherController enemy)
    {
        this._controller = enemy;
    }

    public void EnterState()
    {
        _controller.SetAnimator("Idle", false);
        _controller.SetAnimator("Run", false);
        _controller.SetAnimator("Walk", true);
        _patrolTarget = _controller.GetNextPatrolPoint();
    }

    public void Execute()
    {
        if (_controller.IsPlayerClose())
        {
            _controller.SetState(new EnemyWatcherChaseState(_controller));
        }
        else if (Vector3.Distance(_controller.transform.position, _patrolTarget.position) < 1f)
        {
            _controller.SetState(new EnemyWatcherIdleState(_controller, 3f));
        }
        else
        {
            _controller.MoveTowards(_patrolTarget.position, _controller.PatrolSpeed);
        }
    }
}
