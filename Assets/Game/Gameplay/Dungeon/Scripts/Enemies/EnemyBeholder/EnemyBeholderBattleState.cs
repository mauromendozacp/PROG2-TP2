using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeholderBattleState : IEnemyState
{
    EnemyBeholderController _controller;
    bool _isAttackingMode;

    public EnemyBeholderBattleState(EnemyBeholderController controller)
    {
        this._controller = controller;
    }
    public void EnterState()
    {
        _controller.SetAnimator("Battle", true);
        _controller.SetAnimator("Walk", false);
        _isAttackingMode = false;
        _controller.SetAnimator("Run", false);

    }

    public void Execute()
    {

        if (!_controller.IsPlayerInAttackRange())
        {
            _controller.SetState(new EnemyBeholderChaseState(_controller));
        }
        else if(!_isAttackingMode)
        {
            _isAttackingMode = true;
            _controller.StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (_isAttackingMode)
        {
            int attackNumber = Random.Range(0, 4);
            _controller.TriggerAnimator($"Attack{attackNumber+1}");
            yield return new WaitForSeconds(3.5f);

        }
    }

}
