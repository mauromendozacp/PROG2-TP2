using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWatcherIdleState : IEnemyState
{
    EnemyWatcherController _controller;
    EnemyAnimation _animation;
    private float _idleTime;
    private float _timer;
    EnemyNavigation _navigation;

    public EnemyWatcherIdleState(EnemyWatcherController enemy, EnemyAnimation animation, float idleTime, EnemyNavigation navigation)
    {
        _controller = enemy;
        _animation = animation;
        _idleTime = idleTime;
        _navigation = navigation;

    }

    public void EnterState()
    {
        _animation.SetAnimator("Walk", false);
        _animation.SetAnimator("Run", false);
        _animation.SetAnimator("Idle", true);
        _timer = 0;
    }

    public void Execute()
    {
        _timer += Time.deltaTime;
        if (_controller.IsPlayerClose())
        {
            _controller.SetState(new EnemyWatcherChaseState(_controller, _animation, _navigation));
        }
        else if (_timer >= _idleTime && _controller.HasSufficientPatrolPoints())
        {
            _controller.SetState(new EnemyWatcherPatrolState(_controller, _animation, _navigation));
        }
    }
}
