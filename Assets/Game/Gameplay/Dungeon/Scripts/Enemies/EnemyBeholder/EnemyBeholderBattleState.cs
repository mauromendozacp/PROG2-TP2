using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeholderBattleState : IEnemyState
{
    EnemyBeholderController _controller;
    bool _isAttackingMode;
    Coroutine _attackCoroutine = null;

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
        _controller.ResetAgentDestination();

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
            _controller.StopAllCoroutines();
            //if (_attackCoroutine != null) _controller.StopCoroutine(_attackCoroutine);
            //_attackCoroutine = _controller.StartCoroutine(AttackRoutine());
            _controller.StartCoroutine(AttackRoutine());
        }
        if(!_controller.IsAttacking)
        {
            _controller.LootAtPlayer();
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (_isAttackingMode)
        {
            int attackNumber = Random.Range(0, _controller.AvailableAttacks.Length);
            _controller.TriggerAnimator($"Attack{attackNumber+1}");
            yield return new WaitForSeconds(3.5f);

        }
    }

}
