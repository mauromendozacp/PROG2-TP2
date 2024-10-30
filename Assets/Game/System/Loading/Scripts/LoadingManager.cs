// old

/*
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private bool isLoading = false;
    public LoadingUI loadingUI = null;

    private Action onFinishTransition = null;

    private static readonly Dictionary<SceneGame, string> sceneNames = new Dictionary<SceneGame, string>()
    {
        { SceneGame.Menu, "Menu" },
        //{ SceneGame.Loading, "Loading" },
        { SceneGame.Dungeon, "Dungeon" } 
    };

    public void SetLoadingUI(LoadingUI loadingUI)
    {
        this.loadingUI = loadingUI;
    }

    public void TransitionScene(SceneGame nextScene, Action onComplete = null)
    {
        isLoading = true;
        if (loadingUI == null)
            {
                Debug.LogError("loadingUI is not assigned!");
                return; 
            }


        loadingUI.ToggleUI(true,
            onComplete: () =>
            {
                UnloadScene(GetCurrentScene(),
                    onSuccess: () =>
                    {
                        LoadingScene(nextScene,
                            onSuccess: () =>
                            {
                                SetActiveScene(nextScene);
                                loadingUI.ToggleUI(false,
                                    onComplete: () =>
                                    {
                                        onComplete?.Invoke();

                                        onFinishTransition?.Invoke();
                                        onFinishTransition = null;

                                        isLoading = false;
                                    });
                            });
                    });
            });
    }

    public void SetFinishTransitionCallback(Action callback)
    {
        if (isLoading)
        {
            onFinishTransition += callback;
        }
        else
        {
            callback.Invoke();
        }
    }

    public void LoadingScene(SceneGame scene, Action onSuccess = null)
    {
        if (sceneNames.TryGetValue(scene, out string sceneName))
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            op.completed += (op) =>
            {
                onSuccess?.Invoke();
            };
        }
    }

    private void UnloadScene(SceneGame scene, Action onSuccess = null)
    {
        if (sceneNames.TryGetValue(scene, out string sceneName))
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
            op.completed += (op) =>
            {
                onSuccess?.Invoke();
            };
        }
    }

    private SceneGame GetCurrentScene()
    {
        string currSceneName = SceneManager.GetActiveScene().name;

        foreach (KeyValuePair<SceneGame, string> scene in sceneNames)
        {
            if (scene.Value == currSceneName)
            {
                return scene.Key;
            }
        }

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

*/


using System;
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
        { SceneGame.Dungeon, "Dungeon" } 
    };

    public void SetLoadingUI(LoadingUI loadingUI)
    {

        this.loadingUI = loadingUI;
       // Debug.Log("LoadingUI has been set.");
    }

    public void TransitionScene(SceneGame nextScene, Action onComplete = null)
{
    isLoading = true;

    if (loadingUI == null)
    {
        //Debug.LogError("loadingUI is not assigned!");
        return; 
    }

    // Show the loading canvas
    loadingUI.ToggleUI(true, onComplete: () =>
    {
        // Load the loading scene (it can be an empty or dedicated loading scene)
        LoadingScene(SceneGame.Loading, onSuccess: () =>
        {
            UnloadScene(GetCurrentScene(), onSuccess: () =>
            {
                // Cargar la escena deseada
                LoadingScene(nextScene, onSuccess: () =>
                {
                    // Establecer la escena activa
                    SetActiveScene(nextScene);

                    // Ocultar el canvas de carga
                    loadingUI.ToggleUI(false, onComplete: () =>
                    {
                        onComplete?.Invoke();
                        isLoading = false;
                    });
                });
            });
        });
    });
}


    public void LoadingScene(SceneGame scene, Action onSuccess = null)
    {
        if (sceneNames.TryGetValue(scene, out string sceneName))
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            op.completed += (op) =>
            {
                onSuccess?.Invoke();
            };
        }
    }

    private void UnloadScene(SceneGame scene, Action onSuccess = null)
    {
        if (sceneNames.TryGetValue(scene, out string sceneName))
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
            op.completed += (op) =>
            {
                onSuccess?.Invoke();
            };
        }
    }

    private SceneGame GetCurrentScene()
    {
        string currSceneName = SceneManager.GetActiveScene().name;

        foreach (KeyValuePair<SceneGame, string> scene in sceneNames)
        {
            if (scene.Value == currSceneName)
            {
                return scene.Key;
            }
        }

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
