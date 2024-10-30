//// old
/*
using System;
using System.Collections;

using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private float lerpTime = 0f;

    private void Awake()
    {
        GameManager.Instance.LoadingManager.SetLoadingUI(this);
    }

    public void ToggleUI(bool status, Action onComplete = null)
    {

        if (canvasGroup == null)
    {
        Debug.LogError("canvasGroup is not assigned in LoadingUI!");
        return; // Salir si canvasGroup es null
    }



        StartCoroutine(LoadingCoroutine());
        IEnumerator LoadingCoroutine()
        {
            float timer = 0f;
            float startAlpha = canvasGroup.alpha;
            float targetAlpha = status ? 1f : 0f;

            while (timer < lerpTime)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / lerpTime);

                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = targetAlpha;
            canvasGroup.blocksRaycasts = status;
            onComplete?.Invoke();
        }
    }
}
*/


using System;
using System.Collections;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup loadingCanvasGroup = null; // Canvas de carga
    [SerializeField] private float lerpTime = 0f;

    private void Awake()
    {
        GameManager.Instance.LoadingManager.SetLoadingUI(this);
    }

    public void ToggleUI(bool status, Action onComplete = null)
    {
        StartCoroutine(LoadingCoroutine());

        IEnumerator LoadingCoroutine()
        {
            float timer = 0f;
            float startAlpha = loadingCanvasGroup.alpha;
            float targetAlpha = status ? 1f : 0f;

            while (timer < lerpTime)
            {
                timer += Time.deltaTime;
                loadingCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / lerpTime);
                yield return new WaitForEndOfFrame();
            }

            loadingCanvasGroup.alpha = targetAlpha;
            loadingCanvasGroup.blocksRaycasts = status;
            onComplete?.Invoke();
        }
    }
}
