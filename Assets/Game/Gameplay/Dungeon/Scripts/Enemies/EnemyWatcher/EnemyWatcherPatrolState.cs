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
        //_patrolTarget = _controller.GetNextPatrolPoint();
        _patrolTarget = _controller.MoveTowardsNexttPatrolPoint();
        Debug.Log("Estado Patrulla");
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
        //else
        //{
        //_controller.MoveTowards(_patrolTarget.position, _controller.MoveSpeed);
        //}
        //Debug.Log(Vector3.Distance(_controller.transform.position, _patrolTarget));
    }
}
