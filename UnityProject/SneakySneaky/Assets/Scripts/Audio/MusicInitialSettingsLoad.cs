using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicInitialSettingsLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioMixer mixer;
    void Start()
    {
        Options options = new Options();
        options.LoadSettings();
        mixer.SetFloat("musicVolume", options.musicVolume);
        mixer.SetFloat("masterVolume", options.totalVolume);
        mixer.SetFloat("soundEffectVolume", options.soundEffectVolume);
    }

   
}
