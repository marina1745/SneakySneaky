using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatsButtons : MonoBehaviour
{
    public GameObject ui;
    private GameDataControl saveGame;
    public int myHatId;
    // Start is called before the first frame update
    void Start()
    {
        saveGame = ui.GetComponent<GameDataControl>();
        if (myHatId >= 0 && !saveGame.IsHatCurrentlyAvailable(myHatId))
            this.GetComponent<Button>().interactable = false;
    }

  
}
