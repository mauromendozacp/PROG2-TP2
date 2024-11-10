using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private GameplayUI gameplayUI = null;

    private void Start()
    {
        playerController.Init(ToggleOnPause);
        gameplayUI.Init(ToggleTimeScale, ToggleOffPause);
    }

    private void ToggleOnPause()
    {
        gameplayUI.TogglePause(true);
        ToggleTimeScale(false);
    }

    private void ToggleOffPause()
    {
        ToggleTimeScale(true);
        playerController.TogglePause(false);
    }

    private void ToggleTimeScale(bool status)
    {
        Time.timeScale = status ? 1f : 0f;
    }
}
