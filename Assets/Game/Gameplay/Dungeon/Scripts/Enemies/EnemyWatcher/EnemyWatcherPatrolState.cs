using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyWatcherPatrolState : IEnemyState
{
    private EnemyWatcherController _controller;
    EnemyAnimation _animation;
    private Vector3 _patrolTarget;
    EnemyNavigation _navigation;

    public EnemyWatcherPatrolState(EnemyWatcherController controller, EnemyAnimation animation, EnemyNavigation navigation)
    {
        _controller = controller;
        _animation = animation;
        _navigation = navigation;
    }

    public void EnterState()
    {
        _animation.SetAnimator("Idle", false);
        _animation.SetAnimator("Run", false);
        _animation.SetAnimator("Walk", true);
        _patrolTarget = _controller.MoveTowardsNexttPatrolPoint();
    }

    public void Execute()
    {
        if (_controller.IsPlayerClose())
        {
            _controller.SetState(new EnemyWatcherChaseState(_controller, _animation, _navigation));
        }
        else if (_controller.IsNearPosition(_patrolTarget))
        {
            _controller.SetState(new EnemyWatcherIdleState(_controller, _animation, 3f, _navigation));
        }
    }
}
