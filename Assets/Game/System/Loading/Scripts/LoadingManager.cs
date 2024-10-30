
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private bool isLoading = false;
    public LoadingUI loadingUI = null;

    private static readonly Dictionary<SceneGame, string> sceneNames = new Dictionary<SceneGame, string>()
    {
        { SceneGame.Menu, "Menu" },
        { SceneGame.Gameplay, "Gameplay" }
    };

    public void SetLoadingUI(LoadingUI loadingUI)
    {
        this.loadingUI = loadingUI;
    }

    public void TransitionScene(SceneGame nextScene, Action onComplete = null)
    {
        if (isLoading) return;

        isLoading = true;

        if (loadingUI == null)
        {
            Debug.LogError("loadingUI is not assigned!");
            return;
        }

        // Show the loading canvas and start the scene transition
        loadingUI.ToggleUI(true, onComplete: () =>
        {
            StartCoroutine(LoadSceneCoroutine(nextScene, onComplete));
        });
    }

    private IEnumerator LoadSceneCoroutine(SceneGame nextScene, Action onComplete)
{
    // Load the new scene first
    yield return LoadingScene(nextScene);

    // Unload the current scene after loading the new one
    yield return UnloadScene(GetCurrentScene());

    // Hide the loading canvas and execute the completion action
    loadingUI.ToggleUI(false, onComplete: () =>
    {
        onComplete?.Invoke();
        isLoading = false;
    });
}


    private IEnumerator LoadingScene(SceneGame scene)
    {
        if (sceneNames.TryGetValue(scene, out string sceneName))
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!op.isDone)
            {
                yield return null;
            }
            // Make the new scene the active one
        }
    }

    private IEnumerator UnloadScene(SceneGame scene)
    {
        if (sceneNames.TryGetValue(scene, out string sceneName))
        {
            Scene sceneToUnload = SceneManager.GetSceneByName(sceneName);

            if (sceneToUnload.isLoaded)
            {
                AsyncOperation op = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
                while (!op.isDone)
                {
                    yield return null;
                }


            }
            else
            {
                Debug.LogWarning($"Scene '{sceneName}' is not loaded, aborting load.");
            }
        }
        else
        {
            Debug.LogError($"Scene '{scene}' not found at 'sceneNames'.");
        }
    }

    private SceneGame GetCurrentScene()
    {
        string currSceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"Actual scene name: {currSceneName}");

        foreach (KeyValuePair<SceneGame, string> scene in sceneNames)
        {
            if (scene.Value == currSceneName)
            {
                return scene.Key;
            }
        }

        Debug.LogWarning("No matching scene found in 'sceneNames'.");
        return default;
    }


    private void SetActiveScene(SceneGame scene)
    {
        if (sceneNames.TryGetValue(scene, out string sceneName))
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
    }
}

