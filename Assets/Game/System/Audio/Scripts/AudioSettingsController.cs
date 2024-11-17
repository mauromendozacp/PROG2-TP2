using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        // Cargar valores guardados y asignarlos a los sliders
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1f);
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        // Configurar valores iniciales en el AudioManager
        GameManager.Instance.AudioManager.UpdateMusicVolume(musicVolume);
        GameManager.Instance.AudioManager.UpdateSfxVolume(sfxVolume);

        // Configurar listeners para cambios en tiempo real
        musicSlider.onValueChanged.AddListener(GameManager.Instance.AudioManager.UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(GameManager.Instance.AudioManager.UpdateSfxVolume);
    }

    private void OnDisable()
    {
        // Guardar configuraciones cuando el objeto se desactiva
        SaveAudioSettings();
    }

    public void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SfxVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }
}
