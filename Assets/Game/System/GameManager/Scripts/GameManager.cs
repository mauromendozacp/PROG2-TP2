using System;
using UnityEngine;

public enum SceneGame
{
    Menu,
    Dungeon,
    Loading
}

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [Header("Manager References")]
    [SerializeField] private LoadingManager loadingManager = null;
    [SerializeField] private LoadingUI loadingUI = null; 
    [SerializeField] private AudioManager audioManager = null;

    public LoadingManager LoadingManager => loadingManager;
    public AudioManager AudioManager => audioManager;

    public override void Awake()
    {
        base.Awake();

        if (instance == null)
        {
            // Find LoadingUI only if loadingUI is null
            if (loadingUI == null)
            {
                loadingUI = FindObjectOfType<LoadingUI>();
            }
            
            // Check that loadingUI is not null
            if (loadingUI == null)
            {
                Debug.LogError("LoadingUI not found in the scene!");
                return;
            }

            loadingManager.SetLoadingUI(loadingUI);
            audioManager.Init();
        }
    }

    public void ChangeScene(SceneGame nextScene, Action onComplete = null)
    {
        audioManager.StopCurrentMusic();
        loadingManager.TransitionScene(nextScene, onComplete);
    }
}
