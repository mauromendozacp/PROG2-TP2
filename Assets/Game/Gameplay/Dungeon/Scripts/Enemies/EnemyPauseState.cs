
public class EnemyPauseState : IEnemyState
{
    Enemy _controller;
    public EnemyPauseState(Enemy controller)
    {
        _controller = controller;
    }
    public void EnterState()
    {
        _controller.SetAnimator("Pause", true);
        _controller.ResetAgentDestination();
    }
    
    public void Execute()
    {
        
    }

}
