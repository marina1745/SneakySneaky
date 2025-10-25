using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FridgeDoorMovement : MonoBehaviour
{
    public AudioClip openClip, closeClip;
    public GameObject fridgeDoor;
    public float degreesToRotate = 30;
    public float maxLerpTime = 0.5f;
    private bool doorOpen = false;
    private AudioSource source;
    private bool lerping = false;

    private float currentLerpTime = 0;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        AiNavigation.OnInteractWithFridge += InteractWithFridgeDoor;
    }

    private void OnDisable()
    {
        AiNavigation.OnInteractWithFridge -= InteractWithFridgeDoor;
    }

    public void InteractWithFridgeDoor()
    {
        if (!lerping)
        {
            if (doorOpen)
            {
                source.PlayOneShot(closeClip, 1);

                // fridgeDoor.transform.Rotate(new Vector3(0, -degreesToRotate, 0), Space.Self);
            }
            else
            {
                source.PlayOneShot(openClip, 1);

                // fridgeDoor.transform.Rotate(new Vector3(0, degreesToRotate, 0), Space.Self);
            }
            currentLerpTime = 0;
            lerping = true;
            doorOpen = !doorOpen;

        }
    }
    private void Update()
    {
        if (lerping)
        {
            int factor = doorOpen ? -1 : 1;
            currentLerpTime += Time.deltaTime;
            transform.Rotate(new Vector3(0, Time.deltaTime / maxLerpTime * degreesToRotate * factor, 0), Space.Self);
            if (currentLerpTime >= maxLerpTime)
                lerping = false;

        }
    }
}
