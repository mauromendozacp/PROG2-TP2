using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyBeholderChaseState : IEnemyState
{
    EnemyBeholderController _controller;
    EnemyAnimation _animation;
    EnemyNavigation _navigation;
    EnemyHealth _enemyHealth;
    public EnemyBeholderChaseState(EnemyBeholderController controller, EnemyAnimation animation, EnemyNavigation navigation)
    {
        _controller = controller;
        _animation = animation;
        _navigation = navigation;
    }
    public void EnterState()
    {
        _animation.SetAnimator("Run", true);
        _animation.SetAnimator("Walk", false);
        _animation.SetAnimator("Battle", false);
        _enemyHealth = _controller.gameObject.GetComponent<EnemyHealth>();
        _enemyHealth?.EnableHealthBar();
    }

    public void Execute()
    {
        if(_controller.IsPlayerFar()) {
            _controller.SetState(new EnemyBeholderIdleState(_controller, _animation, _navigation));
        }
        else if(_controller.IsPlayerInAttackRange())
        {
            _controller.SetState(new EnemyBeholderBattleState(_controller, _animation, _navigation));
        }
        else
        {
            _navigation.MoveTowardsPlayer();
        }
    }
}
