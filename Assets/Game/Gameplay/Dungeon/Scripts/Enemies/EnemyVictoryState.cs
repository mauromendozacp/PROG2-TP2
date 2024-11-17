
public class EnemyVictoryState : IEnemyState
{
    Enemy _controller;
    public EnemyVictoryState(Enemy controller)
    {
        _controller = controller;
    }
    public void EnterState()
    {
        _controller.SetAnimator("EnemyVictory");
    }

    public void Execute()
    {

    }

}
