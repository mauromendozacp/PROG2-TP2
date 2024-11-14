using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChestBattleState : IEnemyState
{
    readonly EnemyChestController chest;
    bool isAttackingMode;
    EnemyHealth _enemyHealth;

    public EnemyChestBattleState(EnemyChestController chest)
    {
        this.chest = chest;
    }

    public void EnterState()
    {
        chest.SetAnimator("IdleAttack", true);
        isAttackingMode = false;
        _enemyHealth = chest.gameObject.GetComponent<EnemyHealth>();
        _enemyHealth?.EnableHealthBar();
    }

    public void Execute()
    {
        if (chest.IsPlayerFar())
        {
            isAttackingMode = false;
            _enemyHealth?.DisableHealthBar();
            chest.SetState(new EnemyChestIdleState(chest));
        }
        else if (!chest.IsPlayerInAttackRange())
        {
            chest.StopAllCoroutines();
            isAttackingMode = false;
            chest.SetAnimator("Run", true);
            chest.LootAtPlayer();
            chest.RunTowardsPlayer();
        }
        else
        {
            chest.SetAnimator("Run", false);
            chest.LootAtPlayer();
            chest.ResetAgentDestination();
            if(!isAttackingMode)
            {
                isAttackingMode = true;
                chest.StartCoroutine(AttackRoutine());
            }

        }
    }


    private IEnumerator AttackRoutine()
    {
        while (isAttackingMode)
        {
            string attackType = Random.Range(0, 2) == 0 ? "Attack1" : "Attack2";
            Debug.Log("Ataque");
            chest.TriggerAnimator(attackType);
            //chest.Attack();
            yield return new WaitForSeconds(3.5f);  // Ataca cada 3.5 segundos
            
        }
    }
}
