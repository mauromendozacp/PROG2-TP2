using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeholderIdleState: IEnemyState
{
    EnemyBeholderController _controller;
    EnemyAnimation _animation;
    EnemyNavigation _navigation;
    Timer _timer;
    EnemyHealth _enemyHealth;
    public EnemyBeholderIdleState(EnemyBeholderController controller, EnemyAnimation animation, EnemyNavigation navigation)
    {
        _controller = controller;
        _animation = animation;
        _navigation = navigation;
        _timer = new Timer();
        _controller.StartCoroutine(_timer.StartTimer(_controller.IdleTimeout));
 
    }
    public void EnterState()
    {
        _animation.SetAnimator("Walk", false);
        _animation.SetAnimator("Battle", false);
        _animation.SetAnimator("Run", false);
        _navigation.ResetAgentDestination();
        _enemyHealth = _controller.gameObject.GetComponent<EnemyHealth>();
        _enemyHealth?.DisableHealthBar();
    }

    public void Execute()
    {
        if (!_timer.IsTimerComplete) return;

        if(_controller.IsPlayerClose())
        {
            _controller.SetState(new EnemyBeholderChaseState(_controller, _animation, _navigation));
        }

        else if (!_controller.IsAtHome())
        {
            _animation.SetAnimator("Walk", true);
            _navigation.MoveTo(_controller.HomePosition);
        }
        else
        {
            _animation.SetAnimator("Walk", false);
            _navigation.ResetAgentDestination();
        }
    }
}
