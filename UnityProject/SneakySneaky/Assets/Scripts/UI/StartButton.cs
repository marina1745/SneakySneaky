using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StartButton : MonoBehaviour
{
    AudioSource source;
    

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    void Click()
    {
        source.Play();
        Debug.Log("Test");
    }

}
