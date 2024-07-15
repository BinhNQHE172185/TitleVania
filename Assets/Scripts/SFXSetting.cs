using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider slider;
    private void Start()
    {
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetSFXVolume();
        }
    }
    public void SetSFXVolume()
    {
        float volume = slider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void LoadVolume()
    {
        slider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetSFXVolume();
    }
}
