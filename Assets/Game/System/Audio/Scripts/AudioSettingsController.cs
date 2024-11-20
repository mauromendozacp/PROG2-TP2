using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1f);
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        
        GameManager.Instance.AudioManager.UpdateMusicVolume(musicVolume);
        GameManager.Instance.AudioManager.UpdateSfxVolume(sfxVolume);
        
        musicSlider.onValueChanged.AddListener(GameManager.Instance.AudioManager.UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(GameManager.Instance.AudioManager.UpdateSfxVolume);
    }

    private void OnDisable()
    {
        SaveAudioSettings();
    }

    public void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SfxVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }
}
