using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject menuCanvas; 

    private void Start()
    {
        // Get the GameManager instance at startup
        gameManager = GameManager.Instance;
        //Debug.Log(gameManager); // This should display the GameManager instance in the console
    }

    // Method to handle the start game button
    public void OnStartGameButton()
    {
        // Hide the menu canvas before starting the load
    menuCanvas.SetActive(false); 

    GameManager.Instance.LoadingManager.TransitionScene(SceneGame.Dungeon, () =>
    {
        // Código a ejecutar cuando la transición de escena se complete
    });
    }

    // Other methods to handle other buttons 
}
