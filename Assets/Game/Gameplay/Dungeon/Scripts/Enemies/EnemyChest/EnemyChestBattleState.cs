using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChestBattleState : IEnemyState
{
    readonly EnemyChestController _controller;
    EnemyAnimation _animation;
    EnemyHealth _enemyHealth;
    EnemyNavigation _navigation;

    public EnemyChestBattleState(EnemyChestController controller, EnemyAnimation animation, EnemyNavigation navigation)
    {
        _controller = controller;
        _animation = animation;
        _navigation = navigation;
    }

    public void EnterState()
    {
        _animation.SetAnimator("IdleAttack", true);
        _enemyHealth = _controller.gameObject.GetComponent<EnemyHealth>();
        _enemyHealth?.EnableHealthBar();
    }

    public void Execute()
    {
        if (_controller.IsPlayerFar())
        {
            _enemyHealth?.DisableHealthBar();
            _controller.SetState(new EnemyChestIdleState(_controller, _animation, _navigation));
        }
        else if (!_controller.IsPlayerInAttackRange())
        {
            _animation.SetAnimator("Run", true);
            _controller.LootAtPlayer();
            _navigation.MoveTowardsPlayer();
        }
        else
        {
            _animation.SetAnimator("Run", false);
            _navigation.ResetAgentDestination();
            if(_controller.CanAttack())
            {
                string attackType = Random.Range(0, 2) == 0 ? "Attack1" : "Attack2";
                _animation.SetAnimator(attackType);
                _controller.DidAttack();
            }
            else if(!_controller.IsAttacking)
            {
                _controller.LootAtPlayer();
            }

        }
    }
}
