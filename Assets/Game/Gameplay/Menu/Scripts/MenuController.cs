/* OLD */
/*
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas; 
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private GameObject configCanvas;
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private Slider sfxSlider = null;

    private void Start()
    {
        // Get the GameManager instance at startup

        musicSlider.onValueChanged.AddListener(GameManager.Instance.AudioManager.UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(GameManager.Instance.AudioManager.UpdateSfxVolume);

        musicSlider.value = GameManager.Instance.AudioManager.MusicVolume;
        sfxSlider.value = GameManager.Instance.AudioManager.SfxVolume;
    }

    // Method to handle the start game button
    public void OnStartGameButton()
    {
        // Hide the menu canvas before starting the load
        menuCanvas.SetActive(false);

        GameManager.Instance.ChangeScene(SceneGame.Gameplay);
    }

    // Method to handle the credits canva
    public void ShowCredits()
    {
        menuCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    // Method to handle the back button in the credits canvas
    public void ShowMenu()
    {
        creditsCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        configCanvas.SetActive(false);
    }

    public void ConfigButton()
    {
        menuCanvas.SetActive(false);
        configCanvas.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
*/
/* Application of SOLID principles: 
- No longer responsible for audio configuration and slider logics */


using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private GameObject configCanvas;

   

   
    public void OnStartGameButton()
    {
        menuCanvas.SetActive(false);
        GameManager.Instance.ChangeScene(SceneGame.Gameplay);
    }

   
    public void ShowCredits()
    {
        menuCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void ShowMenu()
    {
        creditsCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        configCanvas.SetActive(false);
    }

    public void ConfigButton()
    {
        menuCanvas.SetActive(false);
        configCanvas.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
