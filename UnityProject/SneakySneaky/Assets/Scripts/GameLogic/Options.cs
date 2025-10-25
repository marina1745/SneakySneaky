using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options
{
    public float soundEffectVolume;
    public float musicVolume;
    public float totalVolume;



    public Options()
    {
        LoadSettings();
    }
    public void LoadSettings()
    {
        soundEffectVolume = PlayerPrefs.GetFloat("soundEffectVolume", -10);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", -10);
        totalVolume = PlayerPrefs.GetFloat("totalVolume", -10);


    }

    public void SaveSettings()
    {

        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("soundEffectVolume", soundEffectVolume);
        PlayerPrefs.SetFloat("totalVolume", totalVolume);

        PlayerPrefs.Save();
    }
}