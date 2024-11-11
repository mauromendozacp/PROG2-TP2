using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioManager audioManager;

    private void Start()
    {
        if (audioManager != null)
        {
            audioManager.Init(); 
        }
        // Initialize sliders with saved values
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1f);
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        // Set AudioManager values
        audioManager.UpdateMusicVolume(musicVolume);
        audioManager.UpdateSfxVolume(sfxVolume);

        // Add listeners to the sliders to update the volume in real-time
        musicSlider.onValueChanged.AddListener((value) => audioManager.UpdateMusicVolume(value));
        sfxSlider.onValueChanged.AddListener((value) => audioManager.UpdateSfxVolume(value));
    }

    public void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SfxVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }

    private void OnDisable()
    {
        SaveAudioSettings();
    }
}
