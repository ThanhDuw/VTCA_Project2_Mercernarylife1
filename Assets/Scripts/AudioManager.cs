using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource; // Nguồn nhạc (Music)
    public AudioSource soundSource; // Nguồn âm thanh (Sound)

    [Header("UI Sliders")]
    public Slider musicSlider; // Slider điều chỉnh âm lượng Music
    public Slider soundSlider; // Slider điều chỉnh âm lượng Sound

    private void Start()
    {
        // Khởi tạo giá trị Slider dựa trên âm lượng hiện tại
        if (musicSource != null)
        {
            musicSlider.value = musicSource.volume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (soundSource != null)
        {
            soundSlider.value = soundSource.volume;
            soundSlider.onValueChanged.AddListener(SetSoundVolume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
    }

    public void SetSoundVolume(float volume)
    {
        if (soundSource != null)
        {
            soundSource.volume = volume;
        }
    }
}