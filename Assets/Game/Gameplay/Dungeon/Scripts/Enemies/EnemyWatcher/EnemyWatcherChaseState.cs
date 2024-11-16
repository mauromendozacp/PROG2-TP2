using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWatcherChaseState : IEnemyState
{
    private EnemyWatcherController _controller;
    EnemyHealth _enemyHealth;

    public EnemyWatcherChaseState(EnemyWatcherController enemy)
    {
        this._controller = enemy;
    }

    public void EnterState()
    {
        _controller.SetAnimator("Walk", false);
        _controller.SetAnimator("Idle", false);
        _controller.SetAnimator("BattleIdle", false);
        _controller.SetAnimator("Run", true);
        _enemyHealth = _controller.GetComponentInChildren<EnemyHealth>();
        _enemyHealth?.EnableHealthBar();
    }

    public void Execute()
    {
        if (_controller.IsPlayerInAttackRange())
        {
            _controller.SetState(new EnemyWatcherBattleState(_controller));
        }
        else if (!_controller.IsPlayerClose() && _controller.HasSufficientPatrolPoints())
        {
            _enemyHealth?.DisableHealthBar();
            _controller.SetState(new EnemyWatcherPatrolState(_controller));
        }
        else
        {
            _controller.MoveTowardsPlayer();
        }
    }
}
