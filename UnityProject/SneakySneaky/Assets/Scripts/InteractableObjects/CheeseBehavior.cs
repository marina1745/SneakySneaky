using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseBehavior : MonoBehaviour
{

    public delegate void EatAction();
    public static event EatAction OnEaten;
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            OnEaten();
            Destroy(this.gameObject);
                
        }
    }
}
