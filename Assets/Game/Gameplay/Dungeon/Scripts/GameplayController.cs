using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private GameplayUI gameplayUI = null;
    [SerializeField] private WinZone winZone = null;
    [SerializeField] private AudioEvent musicEvent = null;
    [SerializeField] private AudioEvent winEvent = null;
    [SerializeField] private AudioEvent loseEvent = null;

    private void Start()
    {
        playerController.Init(ToggleOnPause, gameplayUI.UpdatePlayerHealth, LoseGame);
        gameplayUI.Init(ToggleTimeScale, ToggleOffPause);
        winZone?.Init(VictoryPlayer, WinGame);

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

    private void VictoryPlayer()
    {
        playerController.DisableInput();
        playerController.PlayVictoryAnimation();

        EnemyManager.Instance.OnPlayerVictory();
        GameManager.Instance.AudioManager.StopCurrentMusic(
            onSuccess: () =>
            {
                GameManager.Instance.AudioManager.PlayAudio(winEvent);
            });
    }

    private void LoseGame()
    {
        gameplayUI.OpenLosePanel();
        EnemyManager.Instance.OnPlayerDefeated();

        GameManager.Instance.AudioManager.StopCurrentMusic(
            onSuccess: () =>
            {
                GameManager.Instance.AudioManager.PlayAudio(loseEvent);
            });
    }

    private void WinGame()
    {
        gameplayUI.OpenWinPanel();
    }

    private void ToggleTimeScale(bool status)
    {
        Time.timeScale = status ? 1f : 0f;
    }
}
