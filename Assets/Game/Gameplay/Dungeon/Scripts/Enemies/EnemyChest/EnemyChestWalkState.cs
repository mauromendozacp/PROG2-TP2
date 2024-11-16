using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : IEnemyState
{
    private readonly EnemyChestController chest;

    public EnemyWalkState(EnemyChestController chest)
    {
        this.chest = chest;
    }

    public void EnterState()
    {
         chest.SetAnimator("Walk", true);
    }

    public void Execute()
    {

        if (chest.IsNearItemToProtect())
        {
            chest.SetState(new EnemyChestIdleState(chest));
        }
        else
        {
            chest.LookAtCollectible();
            chest.MoveTowardsCollectible();  // Mover hacia el coleccionable
        }
    }
}
