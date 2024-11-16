using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWatcherBattleState : IEnemyState
{
    private EnemyWatcherController _controller;

    public EnemyWatcherBattleState(EnemyWatcherController enemy)
    {
        this._controller = enemy;
    }

    public void EnterState()
    {
        _controller.SetAnimator("Run", false);
        _controller.SetAnimator("BattleIdle", true);
    }

    public void Execute()
    {
        if (!_controller.IsPlayerInAttackRange())
        {
            _controller.SetState(new EnemyWatcherChaseState(_controller));
        }
        else if(_controller.CanAttack())
        {
            int attackNumber = Random.Range(0, _controller.AvailableAttacks.Length);
            _controller.TriggerAnimator($"Attack{attackNumber + 1}");
            _controller.DidAttack();
        }
        else if(!_controller.IsAttacking)
        {
            _controller.LootAtPlayer();
        }
    }
}
