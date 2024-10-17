using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private DungeonGenerator dungeonGenerator = null;
    [SerializeField] private PlayerController playerController = null;

    private Vector3 startPlayerPosition = Vector3.zero;

    private void Start()
    {
        startPlayerPosition = playerController.transform.position;

        dungeonGenerator.Init();
        dungeonGenerator.MazeGenerator();
    }

    public void ResetGame()
    {
        playerController.ResetPlayer(startPlayerPosition);
        dungeonGenerator.RegenerateDungeon();
    }
}
