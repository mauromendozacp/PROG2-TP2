using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeholderBattleState : IEnemyState
{
    EnemyBeholderController _controller;
    EnemyAnimation _animation;
    EnemyNavigation _navigation;

    public EnemyBeholderBattleState(EnemyBeholderController controller, EnemyAnimation animation, EnemyNavigation navigation)
    {
        _controller = controller;
        _animation = animation;
        _navigation = navigation;
    }
    public void EnterState()
    {
        _animation.SetAnimator("Battle", true);
        _animation.SetAnimator("Walk", false);
        _animation.SetAnimator("Run", false);
        _navigation.ResetAgentDestination();
    }

    public void Execute()
    {

        if (!_controller.IsPlayerInAttackRange())
        {
            _controller.SetState(new EnemyBeholderChaseState(_controller, _animation, _navigation));
        }
        if(_controller.CanAttack())
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
