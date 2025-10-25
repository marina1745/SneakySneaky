using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ChefSoundHandling : MonoBehaviour
{
    private AudioSource source;

    [SerializeField]
    private List<AudioClip> stepClips;
    private int nextClipToCall = 0;
    public AudioClip confusedClip, surprisedClip;
    public List<AudioClip> workingClips;
   

    //copy paste parts from SteppingSoundsHandling
    void Start()
    {
        source = GetComponent<AudioSource>();
        if (stepClips.Count == 0)
        {
            Object[] files = Resources.LoadAll("Audio/Foley/Chef/Steps", typeof(AudioClip));
            source = GetComponent<AudioSource>();
            foreach (Object f in files)
                stepClips.Add((AudioClip)f);
            stepClips.Sort((k, j) => k.name.CompareTo(j.name));
        }
    }


    //copy-paste
    public void PlayStepClip()
    {

        source.PlayOneShot(stepClips[nextClipToCall]);
        if (nextClipToCall % 2 == 0)
            nextClipToCall++;
        else
            nextClipToCall = Random.Range(0, stepClips.Count / 2) * 2;
    }
    public void PlayWorkingClip()
    {
        float probability = 1f / (workingClips.Count + 1);
        bool playedSomething = false;
        foreach(AudioClip clip in workingClips)
        {
            float value = Random.Range(0f, 1f);
            if(value <probability)
            {
                source.PlayOneShot(clip);
                playedSomething = true;
            }
        }
        if (!playedSomething)
            source.PlayOneShot(workingClips[Random.Range(0, workingClips.Count)]);
    }
    public void PlayConfusedClip()
    {
        source.PlayOneShot(confusedClip);
    }
    public void PlaySurprisedClip()
    {
        source.PlayOneShot(surprisedClip);
    }
    public void PlayClipAsOneShot(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

}
