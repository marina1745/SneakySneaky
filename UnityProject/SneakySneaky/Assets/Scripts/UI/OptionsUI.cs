using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    

    private Options options;
    public Slider musicVolumeSlider;
    public Slider audioVolumeSlider;
    public Slider soundEffectSlider;

    public GameObject uiElements;
    public AudioMixer mixer;
   
    private void Start()
    {
        options = new Options();
        initUiElements();
        
    }
    private void initUiElements()
    {
        musicVolumeSlider.value = options.musicVolume;
    }
    public void UpdateMusicVolume()
    {
        options.musicVolume = musicVolumeSlider.value;
        mixer.SetFloat("musicVolume", options.musicVolume);
        
       
    }
    public void UpdateVolume()
    {
        options.totalVolume = audioVolumeSlider.value;
        mixer.SetFloat("masterVolume", options.totalVolume);
    }
    public void UpdateSoundEffectVolume()
    {
        options.soundEffectVolume = soundEffectSlider.value;
        mixer.SetFloat("soundEffectVolume", options.soundEffectVolume);
    }
    public void Cancel()
    {
        ResetSettings();
        uiElements.SetActive(false);
    }
    public void ResetSettings()
    {
        options.LoadSettings();
        mixer.SetFloat("musicVolume", options.musicVolume);
        mixer.SetFloat("masterVolume", options.totalVolume);
        mixer.SetFloat("soundEffectVolume", options.soundEffectVolume);
        soundEffectSlider.value = options.soundEffectVolume;
        musicVolumeSlider.value = options.musicVolume;
        audioVolumeSlider.value = options.totalVolume;
    }
    public void ApplyChanges()
    {
        options.SaveSettings();
        uiElements.SetActive(false);
    }
    
}
