using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyBeholderChaseState : IEnemyState
{
    EnemyBeholderController _controller;
    EnemyHealth _enemyHealth;
    public EnemyBeholderChaseState(EnemyBeholderController controller)
    {
        this._controller = controller;
    }
    public void EnterState()
    {
        _controller.SetAnimator("Run", true);
        _controller.SetAnimator("Walk", false);
        _controller.SetAnimator("Battle", false);
        _enemyHealth = _controller.gameObject.GetComponent<EnemyHealth>();
        _enemyHealth?.EnableHealthBar();
    }

    public void Execute()
    {
        if(_controller.IsPlayerFar()) {
            _controller.SetState(new EnemyBeholderIdleState(_controller));
        }
        else if(_controller.IsPlayerInAttackRange())
        {
            _controller.SetState(new EnemyBeholderBattleState(_controller));
        }
        else
        {
            _controller.MoveTowardsPlayer();
        }
    }
}
