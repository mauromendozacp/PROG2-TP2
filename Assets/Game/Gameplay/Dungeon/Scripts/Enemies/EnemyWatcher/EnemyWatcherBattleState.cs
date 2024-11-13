using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWatcherBattleState : IEnemyState
{
    private EnemyWatcherController _controller;
    private bool _isAttacking;
    //EnemyHealth _enemyHealth;

    public EnemyWatcherBattleState(EnemyWatcherController enemy)
    {
        this._controller = enemy;
    }

    public void EnterState()
    {
        _controller.SetAnimator("Run", false);
        _controller.SetAnimator("BattleIdle", true);
        _isAttacking = false;
        //_enemyHealth = _enemy.gameObject.GetComponent<EnemyHealth>();
        //_enemyHealth?.EnableHealthBar();
    }

    public void Execute()
    {
        if (!_controller.IsPlayerInAttackRange())
        {
            _isAttacking = false;
            //_enemyHealth?.DisableHealthBar();
            _controller.SetState(new EnemyWatcherChaseState(_controller));
        }
        else
        {
            if (!_isAttacking)
            {
                _isAttacking = true;
                _controller.StartCoroutine(AttackRoutine());
            }
            
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (_isAttacking)
        {
            string attackType = Random.Range(0, 2) == 0 ? "Attack1" : "Attack2";
            _controller.TriggerAnimator(attackType);
            yield return new WaitForSeconds(3.5f);  // Ataca cada 3.5 segundos

        }
    }
}
