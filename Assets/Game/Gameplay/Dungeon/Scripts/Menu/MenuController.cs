using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject menuCanvas; 
    [SerializeField] private Canvas creditsCanvas;

    private void Start()
    {
        // Get the GameManager instance at startup
        gameManager = GameManager.Instance;
        creditsCanvas.gameObject.SetActive(false);
    }

    // Method to handle the start game button
    public void OnStartGameButton()
    {
        // Hide the menu canvas before starting the load
    menuCanvas.SetActive(false); 

    GameManager.Instance.LoadingManager.TransitionScene(SceneGame.Gameplay, () =>
    {
        // Code to execute when the scene transition is complete
    });
    }
        // Method to handle the credits canva
      public void ShowCredits()
    {
        menuCanvas.gameObject.SetActive(false);
        creditsCanvas.gameObject.SetActive(true);
    }

    // Method to handle the back button in the credits canvas
    public void ShowMenu()
    {
        creditsCanvas.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
