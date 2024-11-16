using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyWatcherPatrolState : IEnemyState
{
    private EnemyWatcherController _controller;
    private Vector3 _patrolTarget;

    public EnemyWatcherPatrolState(EnemyWatcherController enemy)
    {
        this._controller = enemy;
    }

    public void EnterState()
    {
        _controller.SetAnimator("Idle", false);
        _controller.SetAnimator("Run", false);
        _controller.SetAnimator("Walk", true);
        _patrolTarget = _controller.MoveTowardsNexttPatrolPoint();
    }

    public void Execute()
    {
        if (_controller.IsPlayerClose())
        {
            _controller.SetState(new EnemyWatcherChaseState(_controller));
        }
        else if (_controller.IsNearPosition(_patrolTarget))
        {
            _controller.SetState(new EnemyWatcherIdleState(_controller, 3f));
        }
    }
}
