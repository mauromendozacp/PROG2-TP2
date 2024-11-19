using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChestWalkState : IEnemyState
{
    readonly EnemyChestController _controller;
    EnemyAnimation _animation;
    EnemyNavigation _navigation;

    public EnemyChestWalkState(EnemyChestController controller, EnemyAnimation animation, EnemyNavigation navigation)
    {
        _controller = controller;
        _animation = animation;
        _navigation = navigation;
    }

    public void EnterState()
    {
        _animation.SetAnimator("Walk", true);
    }

    public void Execute()
    {

        if (_controller.IsNearItemToProtect())
        {
            _controller.SetState(new EnemyChestIdleState(_controller, _animation, _navigation));
        }
        else
        {
            _controller.LookAtCollectible();
            _navigation.MoveTo(_controller.ItemToProtect.position);  // Mover hacia el coleccionable
        }
    }
}
