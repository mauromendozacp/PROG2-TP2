using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeholderIdleState: IEnemyState
{
    EnemyBeholderController _controller;
    Timer _timer;
    EnemyHealth _enemyHealth;
    public EnemyBeholderIdleState(EnemyBeholderController controller)
    {
        this._controller = controller;
        _timer = new Timer();
        _controller.StartCoroutine(_timer.StartTimer(_controller.IdleTimeout));
    }
    public void EnterState()
    {
        _controller.SetAnimator("Walk", false);
        _controller.SetAnimator("Battle", false);
        _controller.SetAnimator("Run", false);
        _controller.ResetAgentDestination();
        _enemyHealth = _controller.gameObject.GetComponent<EnemyHealth>();
        _enemyHealth?.DisableHealthBar();
    }

    public void Execute()
    {
        if (!_timer.IsTimerComplete) return;

        if(_controller.IsPlayerClose())
        {
            _controller.SetState(new EnemyBeholderChaseState(_controller));
        }

        else if (!_controller.IsAtHome())
        {
            _controller.SetAnimator("Walk", true);
            _controller.MoveTowardsHome();
        }
        else
        {
            _controller.SetAnimator("Walk", false);
            _controller.ResetAgentDestination();
        }
    }
}
