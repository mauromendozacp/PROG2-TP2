using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeholderBattleState : IEnemyState
{
    EnemyBeholderController _controller;

    public EnemyBeholderBattleState(EnemyBeholderController controller)
    {
        this._controller = controller;
    }
    public void EnterState()
    {
        _controller.SetAnimator("Battle", true);
        _controller.SetAnimator("Walk", false);
        _controller.SetAnimator("Run", false);
        _controller.ResetAgentDestination();
    }

    public void Execute()
    {

        if (!_controller.IsPlayerInAttackRange())
        {
            _controller.SetState(new EnemyBeholderChaseState(_controller));
        }
        if(_controller.CanAttack())
        {
            int attackNumber = Random.Range(0, _controller.AvailableAttacks.Length);
            _controller.SetAnimator($"Attack{attackNumber + 1}");
            _controller.DidAttack();
        }
        else if(!_controller.IsAttacking)
        {
            _controller.LootAtPlayer();
        }
    }
}
