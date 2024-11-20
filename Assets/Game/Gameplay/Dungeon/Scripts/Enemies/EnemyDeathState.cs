public class EnemyDeathState : IEnemyState
{
    Enemy _controller;
    EnemyAnimation _animation;
    EnemyNavigation _navigation;

    public EnemyDeathState(Enemy controller, EnemyAnimation animation, EnemyNavigation navigation)
    {
        _controller = controller;
        _animation = animation;
        _navigation = navigation;
    }
    public void EnterState()
    {
        _animation.SetAnimator("Die", true);
        _navigation.ResetAgentDestination();
    }

    public void Execute() { }
}
