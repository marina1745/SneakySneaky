using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;
using UnityEngine.InputSystem;

public class ObjectWithWater : InteractableObject
{
    public ZibraLiquidEmitter liquidEmitter;
    public int particlePerSecond;
    public GameObject front;
    public GameObject chef;
    private void OnEnable()
    {
        base.OnEnable();
    }
    private void OnDisable()
    {
        base.OnDisable();
    }

    protected override IEnumerator StartRoutine() {
       
        if (startClip != null)
        {
            source.PlayOneShot(startClip);
            yield return new WaitForSeconds(startClip.length);
        }
        liquidEmitter.ParticlesPerSec = particlePerSecond;
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
        source.Stop();
        liquidEmitter.ParticlesPerSec = 0;
        
        
    }
}
