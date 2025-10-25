using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class StopOverarchingMusic: MonoBehaviour
{
    private string nameOfObject = "Music";
    public GameObject musicPrefab;
    public AudioMixerSnapshot defaultSnapshot;
    public void DeactivateDontDestroy( float time)
    {
        GameObject go = GameObject.Find(nameOfObject);
        if(go!=null)
            Destroy(go, time);
    }
    private void Start()
    {
        GameObject go = GameObject.Find(nameOfObject);
        if (go == null)
            Instantiate(musicPrefab);
        defaultSnapshot.TransitionTo(2.5f);
    }
}
