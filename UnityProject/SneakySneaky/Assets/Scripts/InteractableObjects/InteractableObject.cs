using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public abstract class InteractableObject : MonoBehaviour
{
    public GameObject uiInfo;
    protected bool currentlyListening = false,currentlyRunning=false;
    protected PlayerInputActions _controls;
    protected AudioSource source;
    public AudioClip startClip, endClip;

    protected virtual void OnEnable()
    {
        _controls = new PlayerInputActions();
        _controls.Enable();
        _controls.Mouse.Interaction.started += StartInteraction;
        
        _controls.Mouse.Interaction.performed += StartInteraction;
        AiNavigation.OnPutOutInteractableObject += EndInteraction;
        source = this.GetComponent<AudioSource>();
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player"&&!currentlyRunning)
        {
            uiInfo.SetActive(true);
            currentlyListening = true;
        }
        
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            uiInfo.SetActive(false);
            currentlyListening = false;
        }
    }
    public void EndInteraction(GameObject obj)
    {
        if (this.gameObject == obj)
        {
            StartCoroutine(EndRoutine());
            currentlyRunning = false;
        }
           

    }

    public  void StartInteraction(InputAction.CallbackContext context)
    {

        if (currentlyListening)
        {
            uiInfo.SetActive(false);
            currentlyRunning = true;
            StartCoroutine(StartRoutine());

        }
            
    }


    protected virtual void OnDisable()
    {
        _controls.Disable();
        AiNavigation.OnPutOutInteractableObject -= EndInteraction;
    }

    protected abstract IEnumerator StartRoutine();
    protected abstract IEnumerator EndRoutine();
    
}
