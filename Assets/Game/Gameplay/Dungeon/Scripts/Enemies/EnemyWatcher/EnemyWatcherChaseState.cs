using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWatcherChaseState : IEnemyState
{
    private EnemyWatcherController _controller;
    EnemyAnimation _animation;
    EnemyHealth _enemyHealth;
    EnemyNavigation _navigation;

    public EnemyWatcherChaseState(EnemyWatcherController enemy, EnemyAnimation animation, EnemyNavigation navigation)
    {
        _controller = enemy;
        _animation = animation;
        _navigation = navigation;

    }

    public void EnterState()
    {
        _animation.SetAnimator("Walk", false);
        _animation.SetAnimator("Idle", false);
        _animation.SetAnimator("BattleIdle", false);
        _animation.SetAnimator("Run", true);
        _enemyHealth = _controller.GetComponentInChildren<EnemyHealth>();
        _enemyHealth?.EnableHealthBar();
    }

    public void Execute()
    {
        if (_controller.IsPlayerInAttackRange())
        {
            _controller.SetState(new EnemyWatcherBattleState(_controller, _animation, _navigation));
        }
        else if (!_controller.IsPlayerClose() && _controller.HasSufficientPatrolPoints())
        {
            _enemyHealth?.DisableHealthBar();
            _controller.SetState(new EnemyWatcherPatrolState(_controller, _animation, _navigation));
        }
        else
        {
            _navigation.MoveTowardsPlayer();
        }
    }
}
