using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatParentBehavior : MonoBehaviour
{

    private void Awake()
    {
        if (PlayerPrefs.HasKey("SelectedHat"))
            HatsBehavior.currentlySelectedHat = PlayerPrefs.GetInt("SelectedHat");
        else
            HatsBehavior.currentlySelectedHat = -1;
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        HatsBehavior.OnChangeHatId += SetCorrectHatActive;    
    }
    private void OnDisable()
    {
        HatsBehavior.OnChangeHatId -= SetCorrectHatActive;
    }
    private void SetCorrectHatActive(int index)
    {
        if(index>=0)
            this.transform.GetChild(index).gameObject.SetActive(true);
    }
    public void ChangeWornHatIdTo(int index)
    {
        HatsBehavior.ChangeHatIndexTo(index);
    }
    

}
