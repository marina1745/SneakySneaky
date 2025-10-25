using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class MovableObject : MonoBehaviour
{
    public GameObject front;
    public GameObject chef;
    public AudioClip landingClip;
    public AnimationCurve volumeChange;
    public float maxVelForAudio = 2;

    private float distToGround;
    private Collider collider;
    private float tolerance = 0.1f;
    private bool falling = false;
    private AudioSource source;
    private Rigidbody rb;

    public bool locksChef = true;
    
    void Start()
    {
        collider = GetComponent<Collider>();
        // get the distance to ground
        distToGround = collider.bounds.extents.y;
        this.source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(falling&&isGrounded())
        {
            falling = false;
            Debug.Log(gameObject.name+ " landing");
            source.PlayOneShot(landingClip, 1);
            if(locksChef)
                chef.GetComponent<AiNavigation>().Noise(this.gameObject,front);
        }
        else if (!falling && !isGrounded())
        {
            Debug.Log("falling");
            falling = true;
        }
        if (falling)
            source.volume = 0;
        else
            source.volume = volumeChange.Evaluate(Mathf.Min(rb.velocity.magnitude, maxVelForAudio) / maxVelForAudio);
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + tolerance);
    }
}
