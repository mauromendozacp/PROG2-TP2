using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private GameplayUI gameplayUI = null;
    [SerializeField] private AudioEvent musicEvent = null;
    [SerializeField] private AudioEvent winEvent = null;
    [SerializeField] private AudioEvent loseEvent = null;

    private void Start()
    {
        playerController.Init(ToggleOnPause, gameplayUI.UpdatePlayerHealth, LoseGame);
        gameplayUI.Init(ToggleTimeScale, ToggleOffPause);

        GameManager.Instance.AudioManager.PlayAudio(musicEvent);
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

    private void LoseGame()
    {
        gameplayUI.OpenLosePanel();
        GameManager.Instance.AudioManager.PlayAudio(loseEvent);
        EnemyManager.Instance.OnPlayerDefeated();
    }

    private void WinGame()
    {
        gameplayUI.OpenWinPanel();
        playerController.DisableInput();
        GameManager.Instance.AudioManager.PlayAudio(winEvent);
        EnemyManager.Instance.OnPlayerVictory();
    }

    private void ToggleTimeScale(bool status)
    {
        Time.timeScale = status ? 1f : 0f;
    }
}
