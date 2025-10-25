using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public const int LEVELDIFF = 1;
    public GameDataControl saveGame;
    public int myLevel;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
        if (this.saveGame.GetCurrentMaxLevel() < myLevel)
            this.gameObject.SetActive(false);
        else
            text.text = "Level " + (myLevel - LEVELDIFF);

    }

   
}
