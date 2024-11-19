
public class EnemyVictoryState : IEnemyState
{
    Enemy _controller;
    EnemyAnimation _animation;
    public EnemyVictoryState(Enemy controller, EnemyAnimation animation)
    {
        _controller = controller;
        _animation = animation;
    }
    public void EnterState()
    {
        _animation.SetAnimator("EnemyVictory");
    }

    public void Execute()
    {

    }

}
