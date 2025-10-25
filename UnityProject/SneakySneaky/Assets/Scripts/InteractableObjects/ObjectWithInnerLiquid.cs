using LiquidVolumeFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LiquidVolume))]
public class ObjectWithInnerLiquid : InteractableObject
{
    private LiquidVolume volume;
    public float speedInactive, speedActive;
    public float active_turbulence1, active_turbulence2, inactive_turbulence1, inactive_turbulence2;
    public GameObject chef;
    public GameObject front;
    private void Start()
    {
        this.volume = GetComponent<LiquidVolume>();
    }
    
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
        if (startClip != null)
        {
            source.PlayOneShot(startClip);
            yield return new WaitForSeconds(startClip.length);
        }
        volume.speed = speedActive;
        volume.turbulence1 = active_turbulence1;
        volume.turbulence2 = active_turbulence2;
        source.Play();
        chef.GetComponent<AiNavigation>().Confusion(this.gameObject, front);
    }

    protected override IEnumerator EndRoutine()
    {
        source.Stop();
        if (endClip != null)
        {
            source.PlayOneShot(endClip);
            yield return new WaitForSeconds(endClip.length);
        }
        volume.speed = speedInactive;
        volume.turbulence1 = inactive_turbulence1;
        volume.turbulence2 = inactive_turbulence2;
       
    }
}
