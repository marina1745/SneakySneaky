using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatsBehavior : MonoBehaviour
{
    public static int currentlySelectedHat = -1;
    private int myHatID;

    public delegate void ChangeHatId(int index);
    public static event ChangeHatId OnChangeHatId;
    // Start is called before the first frame update
    void Start()
    {
        myHatID = transform.GetSiblingIndex();
        Debug.Log(this.gameObject.name + " index: " + myHatID);
        if (currentlySelectedHat != myHatID)
            this.gameObject.SetActive(false);
        
        
    }
    private void OnEnable()
    {
        OnChangeHatId += CheckIfStillActiveHat;
    }
    private void OnDisable()
    {
        OnChangeHatId -= CheckIfStillActiveHat;
    }
    void CheckIfStillActiveHat(int index)
    {
        if (index != myHatID)
            this.gameObject.SetActive(false);
    }

    public static void ChangeHatIndexTo(int index)
    {
        currentlySelectedHat = index;
        PlayerPrefs.SetInt("SelectedHat", index);
        OnChangeHatId(index);
    }
   

   
}
