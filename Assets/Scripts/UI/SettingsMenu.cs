using UnityEngine;
using UnityEngine.UI; //for slider;

public class SettingsMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (musicSlider != null)
        { //saved value
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        } 

        if (sfxSlider != null)
        { //saved value
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }    
    }

    public void UpdateMusicVol()
    {
        if (AudioManager.audioManager != null)
        {
            AudioManager.audioManager.SetMusicVol(musicSlider.value);
        }
    }

        public void UpdateSFXVol()
    {
        if (AudioManager.audioManager != null)
        {
            AudioManager.audioManager.SetSFXVol(sfxSlider.value);
        }
    }
}
