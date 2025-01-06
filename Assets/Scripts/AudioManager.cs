using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource musicAudioSource;  // Nguồn nhạc

    [Header("UI Elements")]
    public Slider masterVolumeSlider;     // Slider âm lượng tổng thể
    public Slider musicVolumeSlider;      // Slider âm lượng nhạc

    private void Start()
    {
        float savedMasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);

        masterVolumeSlider.value = savedMasterVolume;
        musicVolumeSlider.value = savedMusicVolume;

        UpdateMasterVolume(savedMasterVolume);
        UpdateMusicVolume(savedMusicVolume);

        masterVolumeSlider.onValueChanged.AddListener(UpdateMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
    }

    private void UpdateMasterVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    private void UpdateMusicVolume(float value)
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = value;
        }
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private void OnDestroy()
    {
        masterVolumeSlider.onValueChanged.RemoveListener(UpdateMasterVolume);
        musicVolumeSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
    }
}