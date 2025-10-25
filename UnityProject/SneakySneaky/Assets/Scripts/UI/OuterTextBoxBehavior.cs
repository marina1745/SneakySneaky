using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class OuterTextBoxBehavior : MonoBehaviour
{
    private List<MonoBehaviour> componentsToDisable;
    public List<GameObject> objectsToDisableBehavior = new List<GameObject>();
    public Image imageOfSpeaker;
    public float transitionTime = 0.8f;
    public List<Vector3> cameraPositions;
    public List<Transform> objectToLookAt;
    public List<TextAsset> texts;
    private AudioSource source;
    public GameObject cam;
    private int currentSet = 0;
    public GameObject textObject;
    public List<Sprite> speakerSprites;
    public List<int> spriteNumberOfSpeaker;
    public List<AudioClip> speakerSpeeches;
    private float timeLerped = 0;
    private bool lerping = false;

    public delegate void NeedTimeForText();
    public static event NeedTimeForText OnNeedTimeForText;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        componentsToDisable = new List<MonoBehaviour>();
        
        foreach (GameObject o in objectsToDisableBehavior)
            componentsToDisable.AddRange(o.GetComponents<MonoBehaviour>());
        foreach (MonoBehaviour c in componentsToDisable)
            c.enabled = false;
        if (texts.Count != cameraPositions.Count||  spriteNumberOfSpeaker.Count!= texts.Count|| objectToLookAt.Count != cameraPositions.Count)
            Debug.LogError("Tutorial needs equal texts and cameraPositions and speakerSpeeches");
        TextBoxBehavior.OnClosingBox += NextSet;
        LetTextBoxWriteNext();
        OnNeedTimeForText();
        
    }
    private void OnDisable()
    {
        TextBoxBehavior.OnClosingBox -= NextSet;
    }

    public void NextSet()
    {
        source.Play();
        foreach (Transform t in transform)
            t.gameObject.SetActive(false);
        
        if (currentSet == texts.Count-1)
            DoneWithTexts();
        else
        {
            
            timeLerped = 0;
            currentSet++;
            lerping = true;
           
           
            Vector3 direction = objectToLookAt[currentSet - 1].transform.position - cameraPositions[currentSet - 1];
            Quaternion toRotation = Quaternion.LookRotation(direction);
           
        }
       
    }
    private void Update()
    {
        if(lerping)
        {

            timeLerped = Mathf.Min(timeLerped+Time.deltaTime, transitionTime);
            float ratio = timeLerped / transitionTime;
            cam.transform.position= Vector3.Lerp(cam.transform.position, cameraPositions[currentSet - 1],ratio);

           
            Vector3 direction =  objectToLookAt[currentSet-1].transform.position - cameraPositions[currentSet - 1];
            Quaternion toRotation = Quaternion.LookRotation(direction);
            
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, toRotation, Time.deltaTime*180);
            

            if (ratio==1)
            {
                lerping = false;
                LetTextBoxWriteNext();
                Debug.Log("new rotation: " + transform.rotation+ ", toRotation "+toRotation);
                cam.transform.rotation = toRotation;
                timeLerped = 0;
            }
        }
    }
    private void LetTextBoxWriteNext()
    {
        
        foreach (Transform t in transform)
            t.gameObject.SetActive(true);
        source.PlayOneShot(speakerSpeeches[spriteNumberOfSpeaker[currentSet]]);
        imageOfSpeaker.sprite = speakerSprites[spriteNumberOfSpeaker[currentSet]];
        textObject.GetComponent<TextBoxBehavior>().WriteText(texts[currentSet].text);
        
    }
    public void DoneWithTexts()
    {
        foreach (MonoBehaviour c in componentsToDisable)
            c.enabled = true;

        OnNeedTimeForText();
        
    }
}
