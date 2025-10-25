using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FridgeBehavior : MonoBehaviour 
{
    public GameObject fridgeDoor;
    private FridgeDoorMovement doorMovement;
    private void Start()
    {
        doorMovement = fridgeDoor.GetComponent<FridgeDoorMovement>();
    }

    public void InteractWithFridgeDoor()
    {
        doorMovement.InteractWithFridgeDoor();
    }

}
