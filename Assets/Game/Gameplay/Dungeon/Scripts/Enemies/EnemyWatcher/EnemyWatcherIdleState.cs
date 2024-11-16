using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWatcherIdleState : IEnemyState
{
    private EnemyWatcherController _controller;
    private float _idleTime;
    private float _timer;

    public EnemyWatcherIdleState(EnemyWatcherController enemy, float idleTime)
    {
        this._controller = enemy;
        this._idleTime = idleTime;
    }

    public void EnterState()
    {
        _controller.SetAnimator("Idle", true);
        _timer = 0;
    }

    public void Execute()
    {
        _timer += Time.deltaTime;
        if (_controller.IsPlayerClose())
        {
            _controller.SetState(new EnemyWatcherChaseState(_controller));
        }
        else if (_timer >= _idleTime && _controller.HasSufficientPatrolPoints())
        {
            _controller.SetState(new EnemyWatcherPatrolState(_controller));
        }
    }
}
