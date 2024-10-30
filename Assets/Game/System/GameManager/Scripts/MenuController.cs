using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Referencia al GameManager
    private GameManager gameManager;
    [SerializeField] private GameObject menuCanvas; 

    private void Start()
    {
        // Obtén la instancia del GameManager al iniciar
        gameManager = GameManager.Instance;
        Debug.Log(gameManager); // Esto debería mostrar la instancia de GameManager en la consola
    }

    // Método para manejar el botón de iniciar juego
    public void OnStartGameButton()
    {
        // Hide the menu canvas before starting the load
    menuCanvas.SetActive(false); 

    GameManager.Instance.LoadingManager.TransitionScene(SceneGame.Dungeon, () =>
    {
        // Código a ejecutar cuando la transición de escena se complete
    });
    }

    // Otros métodos para manejar otros botones (si es necesario)
}
