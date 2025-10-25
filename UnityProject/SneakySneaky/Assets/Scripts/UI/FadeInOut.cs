using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public GameObject blackScreen;
    public float fadeSpeed = 1f;
    private bool fadingIn = false, fadingOut = false;
    public bool IsFadeOutDefault = true;
    private Image imgOfScreen;
    private int sceneToChange = -1;
    public bool fadeOnStart = false;
    private void Start()
    {
        if(blackScreen==null)
        {
            Transform bsTransform = this.transform.Find("BlackScreen");
           
            //Create new blackscreen
            if(bsTransform==null)
            {
                blackScreen = Instantiate((GameObject)Resources.Load("Prefabs/UI/BlackScreen", typeof(GameObject)));
                blackScreen.transform.SetParent(this.transform);
                blackScreen.GetComponent<RectTransform>().offsetMax = Vector2.zero;
                blackScreen.GetComponent<RectTransform>().offsetMax = Vector2.zero;

            }
            else
            {
                blackScreen = bsTransform.gameObject;
            }
        }
        imgOfScreen = blackScreen.GetComponent<Image>();
        if (!IsFadeOutDefault)
            imgOfScreen.color = new Color(imgOfScreen.color.r, imgOfScreen.color.g, imgOfScreen.color.b, 1);
        else
            imgOfScreen.color = new Color(imgOfScreen.color.r, imgOfScreen.color.g, imgOfScreen.color.b, 0);

        if (fadeOnStart)
            Fade();
        
    }

    public void Fade()
    {
        Debug.Log(imgOfScreen.color.a);
        if (!fadingOut && !fadingIn)
        {
            if (imgOfScreen.color.a == 0)
                fadingOut = true;
            else
                fadingIn = true;
        }
    }
    public void FadeWithSceneChange(int indexOfScene)
    {
        Time.timeScale = 1;
        sceneToChange = indexOfScene;
        Fade();
    }
    public void ChangeFadeSpeed(float newSpeed)
    {
        fadeSpeed = newSpeed;
    }
    public void Update()
    {
        if(fadingOut)
        {
            imgOfScreen.color = new Color(imgOfScreen.color.r, imgOfScreen.color.g, imgOfScreen.color.b, 
                Mathf.Min(1, imgOfScreen.color.a+fadeSpeed*Time.deltaTime));
            if (imgOfScreen.color.a == 1)
            {
                fadingOut = false;
                if (sceneToChange >= 0)
                    SceneManager.LoadScene(sceneToChange);
            }

        }else if(fadingIn)
        {
            imgOfScreen.color = new Color(imgOfScreen.color.r, imgOfScreen.color.g, imgOfScreen.color.b,
                Mathf.Max(0, imgOfScreen.color.a - fadeSpeed * Time.deltaTime));
            if (imgOfScreen.color.a == 0)
            {
                fadingIn = false;
                if (sceneToChange >= 0)
                    SceneManager.LoadScene(sceneToChange);
            }
        }
    }

}
