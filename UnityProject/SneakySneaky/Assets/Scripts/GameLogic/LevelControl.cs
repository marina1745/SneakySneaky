using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(GameDataControl))]
public class LevelControl : MonoBehaviour
{

    public GameObject cheeseParent;
    public static int LastLevelPlayed;
    private int currentCheeseScore=0, totalCheese;
    public int timeForLevel = 60;
    public GameObject winningScreen,losingScreen;
    public GameObject currentCheeseScoreGameObject;
    private Text currentCheeseScoreText;

    public Text timeLeftForLevelText;
    private bool gameOver=false, gameWon=false;
    public AudioSource playerAudioSource;
    public List<AudioClip> eatingSounds;
    private GameDataControl saveGame;
    public GameObject menu;
    private bool performCheckLosingCondition = true;
    
    private void Awake()
    {
        OuterTextBoxBehavior.OnNeedTimeForText += ToggleCheckLosingCondition;
        InvokeRepeating("CheckLosingCondition", 1, 1);
    }
    void Start()
    {
        LastLevelPlayed = -1;
        if (cheeseParent == null)
            cheeseParent = this.transform.Find("Cheese").gameObject;
        if(cheeseParent!=null)
        {
            totalCheese = cheeseParent.transform.childCount;
        }
        CheeseBehavior.OnEaten += CheckWinningCondition;
        currentCheeseScoreText = currentCheeseScoreGameObject.GetComponent<Text>();
        UpdateUI(); 
        saveGame = GetComponent<GameDataControl>();
        AiNavigation.OnCaughtMouse +=LoseGame;
        MouseEnteringFireHandling.OnMouseEnteringFire += LoseGame;
    }
    private void OnDisable()
    {
        OuterTextBoxBehavior.OnNeedTimeForText -= ToggleCheckLosingCondition;
        CancelInvoke();
        CheeseBehavior.OnEaten -= CheckWinningCondition;
        AiNavigation.OnCaughtMouse -= LoseGame;
        MouseEnteringFireHandling.OnMouseEnteringFire -= LoseGame;
    }


    void ToggleCheckLosingCondition()
    {
        if (performCheckLosingCondition)
            CancelInvoke();
        else
            InvokeRepeating("CheckLosingCondition", 1, 1);
        performCheckLosingCondition = !performCheckLosingCondition;
    }
    
    void CheckLosingCondition()
    {

        if (gameOver||gameWon)
            CancelInvoke();
        else
        {
            if (--timeForLevel == 0)
                LoseGame();
        }
        UpdateUI();
    }

    void LoseGame()
    {
        gameOver = true;
        CancelInvoke();
        UpdateUI();
    }
    void CheckWinningCondition()
    {

        if (gameOver)
            return;
        currentCheeseScore++;
        if (currentCheeseScore == totalCheese)
            gameWon=true;
        if(!gameWon)
        {
            playerAudioSource.PlayOneShot(eatingSounds[Random.Range(0, eatingSounds.Count)]);
        }
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (gameWon || gameOver)
        {
           // Time.timeScale = 0;
            currentCheeseScoreGameObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            menu.SetActive(false);
            if (gameWon)
            {
                winningScreen.SetActive(true);
                saveGame.LevelWon(SceneManager.GetActiveScene().buildIndex);
                LastLevelPlayed = SceneManager.GetActiveScene().buildIndex;
            }
            else
                losingScreen.SetActive(true);
        }      
        currentCheeseScoreText.text = currentCheeseScore + "/" + totalCheese;
        timeLeftForLevelText.text = ""+timeForLevel; 

    }

    
}
