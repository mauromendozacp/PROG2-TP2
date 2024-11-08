using System;

using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [Header("HUD Settings")]
    [SerializeField] private Slider playerHealthSlider = null;

    [Header("Pause Settings")]
    [SerializeField] private GameObject pausePanel = null;
    [SerializeField] private Button resumeBtn = null;
    [SerializeField] private Button backToMenuBtn = null;

    private Action<bool> onTogglePlayerInput = null;

    private void Start()
    {
        resumeBtn.onClick.AddListener(() => TogglePause(false));
        backToMenuBtn.onClick.AddListener(BackToMenu);
    }

    public void Init(Action<bool> onTogglePlayerInput)
    {
        this.onTogglePlayerInput = onTogglePlayerInput;
    }

    public void TogglePause(bool status)
    {
        pausePanel.SetActive(status);
        ToggleTimeScale(!status);
        onTogglePlayerInput?.Invoke(status);
    }

    public void UpdatePlayerHealth(int currentLives, int maxLives)
    {
        playerHealthSlider.value = (float)currentLives / maxLives;
    }

    private void BackToMenu()
    {
        resumeBtn.interactable = false;
        backToMenuBtn.interactable = false;

        GameManager.Instance.ChangeScene(SceneGame.Menu);
        GameManager.Instance.AudioManager.ToggleMusic(true);
        ToggleTimeScale(true);
    }

    private void ToggleTimeScale(bool status)
    {
        Time.timeScale = status ? 1f : 0f;
    }
}