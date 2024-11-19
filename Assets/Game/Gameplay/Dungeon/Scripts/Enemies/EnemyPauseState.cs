
public class EnemyPauseState : IEnemyState
{
    Enemy _controller;
    EnemyAnimation _animation;
    EnemyNavigation _navigation;
    public EnemyPauseState(Enemy controller, EnemyAnimation animation, EnemyNavigation navigation)
    {
        _controller = controller;
        _animation = animation;
        _navigation = navigation;
    }
    public void EnterState()
    {
        _animation.SetAnimator("Pause", true);
        _navigation.ResetAgentDestination();
    }
    
    public void Execute()
    {
        
    }

}
