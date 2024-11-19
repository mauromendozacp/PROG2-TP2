using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyChestIdleState : IEnemyState
{
    readonly EnemyChestController _controller;
    EnemyAnimation _animation;
    EnemyNavigation _navigation;
    public EnemyChestIdleState(EnemyChestController controller, EnemyAnimation animation, EnemyNavigation navigation)
    {
        _controller = controller;
        _animation = animation;
        _navigation = navigation;
    }

    public void EnterState()
    {
        _animation.SetAnimator("Sleep", false);
        _animation.SetAnimator("Walk", false);
        _animation.SetAnimator("IdleAttack", false);
        _animation.SetAnimator("Run", false);
        _navigation.ResetAgentDestination();

    }

    public void Execute()
    {
        if(_controller.IsNearItemToProtect() && !_controller.IsPlayerClose())
        {
            _controller.SetState(new EnemyChestSleepingState(_controller, _animation, _navigation));
        }
        else if(!_controller.IsNearItemToProtect())
        {
            _controller.SetState(new EnemyChestWalkState(_controller, _animation, _navigation));
        }
        if (_controller.IsNearItemToProtect() && _controller.IsPlayerClose())
        {
            _controller.SetState(new EnemyChestBattleState(_controller, _animation, _navigation));
        }
    }
}
