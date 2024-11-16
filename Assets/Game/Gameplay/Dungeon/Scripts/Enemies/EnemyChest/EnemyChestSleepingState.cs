using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChestSleepingState : IEnemyState
{
    private readonly EnemyChestController _controller;

    public EnemyChestSleepingState(EnemyChestController controller)
    {
        _controller = controller;
    }

    public void EnterState()
    {
        _controller.ResetAgentDestination();
        _controller.SetAnimator("Sleep", true);
    }

    public void Execute()
    {
        if (!_controller.IsNearItemToProtect() || _controller.IsPlayerClose())
        {
            _controller.SetState(new EnemyChestIdleState(_controller));
        }
    }
}
