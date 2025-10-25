using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

[RequireComponent(typeof(AudioSource))]
public class PauseMenuControl : MonoBehaviour
{
    
    public List<GameObject> objectsToToggleActive;
    private PlayerInputActions _actions;
    
    private List<AudioSource> soundEffects;
    public AudioMixerSnapshot defaultSnapshot, optionsSnapshot;
    private bool menuOpen = false, soundsOpen = false;
    public AudioMixerGroup soundEffectsAudioMixerGroup;
    private AudioSource menuAudioSource;
    public GameObject ui,menuUI, soundUI;
    public AudioClip openClip, closeClip;
    private void Start()
    {
        _actions = new PlayerInputActions();
        _actions.Enable();
        _actions.Menu.ToggleMenu.started += ToggleMenu;
        soundEffects = new List<AudioSource>(FindObjectsOfType<AudioSource>());
        soundEffects.RemoveAll(s => s.outputAudioMixerGroup != soundEffectsAudioMixerGroup);
        List<AudioSource> uiSounds = new List<AudioSource>(ui.GetComponentsInChildren<AudioSource>());
        soundEffects.RemoveAll(s=>uiSounds.Contains(s));
        menuAudioSource = GetComponent<AudioSource>();
        
       
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        menuOpen = !menuOpen;
        if(!menuOpen)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            defaultSnapshot.TransitionTo(0.3f);
            menuAudioSource.PlayOneShot(closeClip);
            menuUI.SetActive(true);
            soundUI.SetActive(false);
            foreach (AudioSource s in soundEffects)
                if (s != null)
                    s.UnPause();
            foreach (AudioSource s in soundEffects)
                if (s != null)
                    s.mute = false;
        }
        else
        {
            Cursor.visible = true;
           Cursor.lockState = CursorLockMode.None;
            optionsSnapshot.TransitionTo(0);
            menuAudioSource.PlayOneShot(openClip );
            Time.timeScale = 0;
            foreach (AudioSource s in soundEffects)
                if (s != null)
                    s.Pause();
            foreach (AudioSource s in soundEffects)
                if (s != null)
                    s.mute=true;
        }
        foreach (GameObject o in objectsToToggleActive)
            o.SetActive(!o.activeSelf);

    }
    public void SceneChange()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;   
    }
    public void ToggleSoundOptions()
    {
        menuUI.SetActive(!menuUI.activeSelf);
        if (!soundsOpen)
            soundUI.SetActive(true);
        soundsOpen = !soundsOpen;
    }
    private void OnDisable()
    {
        _actions.Disable();
        _actions.Menu.ToggleMenu.started -= ToggleMenu;
    }
}
