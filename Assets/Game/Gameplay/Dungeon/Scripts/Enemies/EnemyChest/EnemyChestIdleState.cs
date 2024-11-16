using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChestIdleState : IEnemyState
{
    private readonly EnemyChestController chest;

    public EnemyChestIdleState(EnemyChestController chest)
    {
        this.chest = chest;
    }

    public void EnterState()
    {
        chest.SetAnimator("Sleep", false);
        chest.SetAnimator("Walk", false);
        chest.SetAnimator("IdleAttack", false);
        chest.SetAnimator("Run", false);
        chest.ResetAgentDestination();

    }

    public void Execute()
    {
        if(chest.IsNearItemToProtect() && !chest.IsPlayerClose())
        {
            chest.SetState(new EnemyChestSleepingState(chest));
        }
        else if(!chest.IsNearItemToProtect())
        {
            chest.SetState(new EnemyWalkState(chest));
        }
        if (chest.IsNearItemToProtect() && chest.IsPlayerClose())
        {
            chest.SetState(new EnemyChestBattleState(chest));
        }
    }
}
