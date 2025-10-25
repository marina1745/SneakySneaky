using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextBoxBehavior : MonoBehaviour
{
    private Text text;
 
    public float textSpeed= 0.1f;
    public float fastTextSpeedFactor = 0.62f;
    private PlayerInputActions _controls;
    private string currentWord;
    private int currentLetterPosition = 0;
    private List<string> wordsLeftToWrite;
    public int numberMaxLines = 3;
    private bool  waitingForContinue = false;

    public delegate void ClosingBox();
    public static event ClosingBox OnClosingBox;

    // Start is called before the first frame update
    void OnEnable()
    {
        _controls = new PlayerInputActions();
        _controls.Enable();
        
        _controls.TextBox.Continue.started += MousePressed;
        _controls.TextBox.Continue.canceled += MouseStoppedBeingPressed;

        waitingForContinue = false;
    }
    private void Awake()
    {
        text = GetComponent<Text>();
    }
    public void MousePressed(InputAction.CallbackContext context)
    {
        ContinueNextPart();
        CancelInvoke();
       
        InvokeRepeating("WriteNextLetter", textSpeed * fastTextSpeedFactor, textSpeed * fastTextSpeedFactor);
        
    }
    private void WriteNextLetter()
    {
        if (!waitingForContinue&&Time.timeScale>0)
        {
            
            if (currentWord.Length == currentLetterPosition)
            {
                currentLetterPosition = 0;
                text.text += " ";
                if (wordsLeftToWrite.Count == 0)
                {
                    CancelInvoke();
                    waitingForContinue = true;
                }
                else
                {
                    currentWord = (string)wordsLeftToWrite[0].Clone();
                    wordsLeftToWrite.RemoveAt(0);

                    int currentLines = text.cachedTextGenerator.lineCount;
                    string textSoFar = text.text;
                    text.text += currentWord;
                   
                    Canvas.ForceUpdateCanvases();
                    int lines = text.cachedTextGenerator.lineCount;
                    
                    if(lines > currentLines&&currentLines!=0)
                    {
                        Debug.Log("lines: " + lines + ", currentLines: " + currentLines);
                        if (lines > numberMaxLines)
                            waitingForContinue = true;
                        text.text = textSoFar + "\n";
                        
                    }
                    else
                    {
                        text.text = textSoFar;
                    }
                    Canvas.ForceUpdateCanvases();

                }
            }
            if (!waitingForContinue)
            {
                text.text += currentWord[currentLetterPosition];
                currentLetterPosition++;
                Canvas.ForceUpdateCanvases();
            }
            
        }
    }
    private void ContinueNextPart()
    {
        if(waitingForContinue && Time.timeScale > 0)
        {
            
            text.text = "";
            Canvas.ForceUpdateCanvases();
            if (wordsLeftToWrite.Count == 0)
            {
                OnClosingBox();
                
            }
           
            waitingForContinue = false;
        }
    }
    public void MouseStoppedBeingPressed(InputAction.CallbackContext context)
    {
        StartDefaultWriting();
       
    }
    private void StartDefaultWriting()
    {
        
        CancelInvoke();
        InvokeRepeating("WriteNextLetter", textSpeed, textSpeed);
    }

    public void WriteText(string textToWrite)
    {
        if (text == null)
            text = GetComponent<Text>();
        wordsLeftToWrite=new List<string>(textToWrite.Split(' ','\n'));
        currentWord = wordsLeftToWrite[0];
        wordsLeftToWrite.RemoveAt(0);
        currentLetterPosition = 0;
        text.text = "";
        waitingForContinue = false;
        StartDefaultWriting();
    }

    private void OnDisable()
    {
        CancelInvoke();
        _controls.Disable();
    }


}
