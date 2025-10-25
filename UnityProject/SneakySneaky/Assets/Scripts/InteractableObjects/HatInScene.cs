using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatInScene : MonoBehaviour
{
    public GameObject objectWithSaveData;
    private GameDataControl saveGame;
    public AudioSource audioSourceOfPlayer;
    public AudioClip collectionClip;
    public int myHatId;
    private void Start()
    {
        saveGame = objectWithSaveData.GetComponent<GameDataControl>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            audioSourceOfPlayer.PlayOneShot(collectionClip);
            saveGame.FoundHat(myHatId);
            this.gameObject.SetActive(false);
        }
    }
}
