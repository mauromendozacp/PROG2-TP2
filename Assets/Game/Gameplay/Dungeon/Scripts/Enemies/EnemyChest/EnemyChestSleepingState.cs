using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChestSleepingState : IEnemyState
{
    readonly EnemyChestController _controller;
    EnemyAnimation _animation;
    EnemyNavigation _navigation;

    public EnemyChestSleepingState(EnemyChestController controller, EnemyAnimation animation, EnemyNavigation navigation)
    {
        _controller = controller;
        _animation = animation;
        _navigation = navigation;
    }

    public void EnterState()
    {
        _navigation.ResetAgentDestination();
        _animation.SetAnimator("Sleep", true);
    }

    public void Execute()
    {
        if (!_controller.IsNearItemToProtect() || _controller.IsPlayerClose())
        {
            _controller.SetState(new EnemyChestIdleState(_controller, _animation, _navigation));
        }
    }
}
