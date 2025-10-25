using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectWithFire : InteractableObject
{

    public GameObject fire;
    public GameObject chef;
    public GameObject front;
    
    private void OnEnable()
    {
        base.OnEnable();
    }
    private void OnDisable()
    {
        base.OnDisable();
    }

    protected override IEnumerator StartRoutine()
    {
        fire.SetActive(true);
        
        if (startClip != null)
        {
            source.PlayOneShot(startClip);
            yield return new WaitForSeconds(startClip.length);
        }
        chef.GetComponent<AiNavigation>().Confusion(gameObject,front);
        source.Play();
       
    }

    protected override IEnumerator EndRoutine()
    {
        if (endClip != null)
        {
            source.PlayOneShot(endClip);
            yield return new WaitForSeconds(endClip.length);
        }
        fire.SetActive(false);
        source.Stop();
    }
}
