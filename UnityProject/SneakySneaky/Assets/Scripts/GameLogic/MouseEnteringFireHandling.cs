using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MouseEnteringFireHandling : MonoBehaviour
{
    public delegate void MouseEnteringFire();
    public static event MouseEnteringFire OnMouseEnteringFire;
    public AudioClip mouseBurningAudioClip;
    public AudioSource source;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnMouseEnteringFire();
            source.PlayOneShot(mouseBurningAudioClip);
        }
        
    }
}
