using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWatcherBattleState : IEnemyState
{
    EnemyWatcherController _controller;
    EnemyAnimation _animation;
    EnemyNavigation _navigation;
    public EnemyWatcherBattleState(EnemyWatcherController enemy, EnemyAnimation animation, EnemyNavigation navigation)
    {
        _controller = enemy;
        _animation = animation;
        _navigation = navigation;
    }

    public void EnterState()
    {
        _animation.SetAnimator("Run", false);
        _animation.SetAnimator("BattleIdle", true);
    }

    public void Execute()
    {
        if (!_controller.IsPlayerInAttackRange())
        {
            _controller.SetState(new EnemyWatcherChaseState(_controller, _animation, _navigation));
        }
        else if(_controller.CanAttack())
        {
            int attackNumber = Random.Range(0, _controller.AvailableAttacks.Length);
            _animation.SetAnimator($"Attack{attackNumber + 1}");
            _controller.DidAttack();
        }
        else if(!_controller.IsAttacking)
        {
            _controller.LootAtPlayer();
        }
    }
}
