using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SnapshotTransitionWhenStartingScene : MonoBehaviour
{
    public AudioMixerSnapshot snapshot;
    public float transitionTime;
    // Start is called before the first frame update
    void Start()
    {
        snapshot.TransitionTo(transitionTime);
    }


}
