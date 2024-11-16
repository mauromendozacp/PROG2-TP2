using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChestBattleState : IEnemyState
{
    readonly EnemyChestController _controller;
    EnemyHealth _enemyHealth;

    public EnemyChestBattleState(EnemyChestController controller)
    {
        _controller = controller;
    }

    public void EnterState()
    {
        _controller.SetAnimator("IdleAttack", true);
        _enemyHealth = _controller.gameObject.GetComponent<EnemyHealth>();
        _enemyHealth?.EnableHealthBar();
    }

    public void Execute()
    {
        if (_controller.IsPlayerFar())
        {
            _enemyHealth?.DisableHealthBar();
            _controller.SetState(new EnemyChestIdleState(_controller));
        }
        else if (!_controller.IsPlayerInAttackRange())
        {
            _controller.SetAnimator("Run", true);
            _controller.LootAtPlayer();
            _controller.MoveTowardsPlayer();
        }
        else
        {
            _controller.SetAnimator("Run", false);
            _controller.ResetAgentDestination();
            if(_controller.CanAttack())
            {
                string attackType = Random.Range(0, 2) == 0 ? "Attack1" : "Attack2";
                _controller.TriggerAnimator(attackType);
                _controller.DidAttack();
            }
            else if(!_controller.IsAttacking)
            {
                _controller.LootAtPlayer();
            }

        }
    }
}
