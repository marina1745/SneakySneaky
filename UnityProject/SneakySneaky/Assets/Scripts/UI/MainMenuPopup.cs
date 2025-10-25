using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))]
public class MainMenuPopup : MonoBehaviour
{
    public Text popupText;
    public GameObject popupBox;
    public TextAsset levelsClearedText, controlsText;
    private AudioSource source;
    
    // Start is called before the first frame update
    void Start()
    {

        this.source = GetComponent<AudioSource>();
        if (LevelControl.LastLevelPlayed == SceneManager.sceneCountInBuildSettings - 2)
        {
            popupText.text = levelsClearedText.text;
            ShowPopup();

        }
    }
    public void ShowControlsText()
    {
        popupText.text = controlsText.text;
        ShowPopup();
        
    }
    private void ShowPopup()
    {
        popupBox.SetActive(true);
        source.Play();
    }
   

}
