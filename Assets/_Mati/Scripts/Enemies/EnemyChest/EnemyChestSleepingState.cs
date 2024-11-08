using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChestSleepingState : IEnemyState
{
    private readonly EnemyChestController chest;

    public EnemyChestSleepingState(EnemyChestController chest)
    {
        this.chest = chest;
    }

    public void EnterState()
    {
        chest.SetAnimator("Sleep", true);
    }

    public void Execute()
    {
        if (!chest.IsNearCollectible() || chest.IsPlayerClose())
        {
            chest.SetState(new EnemyChestIdleState(chest));
        }
    }
}
