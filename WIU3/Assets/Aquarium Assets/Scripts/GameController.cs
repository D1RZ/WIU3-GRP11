using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Image conditionBar;

    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI bannerText;
    [SerializeField] GameObject MakeScreenDarkerPanel;
    [SerializeField] GameObject EndGameUI;

    [SerializeField] private float _timeRemaining = 120;
    [SerializeField] public float maxCondition = 200f;
    GameObject[] Seaweeds = null;
    GameObject[] smallFishes = null;
    GameObject[] bigFishes = null;
    GameObject[] fishFood = null;

    private bool isPaused = false;
    private float _timeElapsed = 0;
    public float currentCondition;
    int SeaweedCount = 0;
    int smallFishesCount = 0;
    int bigFishesCount = 0;

    [SerializeField] GameObject AudioSettingsPanel;

    private AudioSettingsManager audioSettingsManager;

    private void Start()
    {
        GameStatus("Start");
        currentCondition = maxCondition;
        audioSettingsManager = AudioSettingsPanel.GetComponent<AudioSettingsManager>();
        audioSettingsManager.Load();
        AudioSettingsPanel.SetActive(false);
        GameStatus("Go");
    }

    void Update()
    {
        audioSettingsManager.Awake();
        GetCounts();
        EventTriggers();
        UpdateConditionBar();
        UpdateTimer();
        _timeElapsed += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.I)) // Only works if you press I to close instead of pressing close button
        {
            if (!AudioSettingsPanel.activeInHierarchy)
            {
                isPaused = true;
                AudioSettingsPanel.SetActive(true);
            }
            else
            {
                AudioSettingsPanel.SetActive(false);
                isPaused = false;
            } 
        }

        if (isPaused == true)
            GameStatus("Stop");
        else if (isPaused == false)
            GameStatus("Go");
    }

    private void GetCounts()
    {
        // Find all active objects
        Seaweeds = GameObject.FindGameObjectsWithTag("Seaweed");
        smallFishes = GameObject.FindGameObjectsWithTag("SmallFish");
        bigFishes = GameObject.FindGameObjectsWithTag("BigFish");

        // Get the counts of each type
        SeaweedCount = Seaweeds.Length;
        smallFishesCount = smallFishes.Length;
        bigFishesCount = bigFishes.Length;
    }

    private void EventTriggers()
    {
        // Decay Condition Triggers
        if (SeaweedCount <= 0) // Too little Seaweed
        {
            DecayCondition(5);
        }
        if (smallFishesCount > 20 || bigFishesCount > 5) // Too many Small or Big Fishes
        {
            DecayCondition(10);
        }
        if (smallFishesCount == 0 || bigFishesCount == 0) // Too few Small or Big Fishes
        {
            DecayCondition(30);
        }

        // Lose Condition
        if (currentCondition <= 0)
        {
            // Call end screen
            bannerText.text = "Better luck next time!";
            isPaused = true;
            //GameStatus("Stop");
            GameStatus("End");
        }

        // Timer end Trigger
        if (_timeRemaining <= 0)
        {
            // Call end screen
            bannerText.text = "Well Done!";
            //isPaused = true;
            //GameStatus("Stop");
            GameStatus("End");
        }
    }

    private void DecayCondition(float reduceAmt) // Reduce this amount of condition continuously
    {
        // Code to reduce currentCondition over time
        currentCondition -= reduceAmt * Time.deltaTime;
        currentCondition = Mathf.Clamp(currentCondition, 0, maxCondition); // Ensure it doesn’t go below 0 or above maxCondition
    }

    private void UpdateConditionBar()
    {
        conditionBar.fillAmount = Mathf.Clamp(currentCondition / maxCondition, 0, 1);
    }

    private void GameStatus(string gameStatus) // Maybe could use a StateMachine for this
    {
        if (gameStatus == "Start")
        {
            // Initialize stuff (again)?
            // Reload the scene? This state might not be needed
        }
        else if (gameStatus == "Go")
        {
            Debug.Log("Go gamestatus");
            Time.timeScale = 1;
        }
        else if (gameStatus == "Stop")
        {
            Debug.Log("Stop gamestatus AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAaaa");
            Time.timeScale = 0;
            // Stop music
        }
        else if (gameStatus == "End")
        {  // -------------------------------------------------------------------------------------
            Time.timeScale = 1f;
            // Check score
            if (_timeRemaining >= 60) // 1 stars
                Debug.Log("1 star loser");
            else if (_timeRemaining > 20 && _timeRemaining <= 60) // 2 stars
                Debug.Log("kys but good try");
            else if (_timeRemaining <= 20) // 3 stars
                Debug.Log("Ok tryhard");
            // Show Endgame Screen
            MakeScreenDarkerPanel.SetActive(true);
            EndGameUI.SetActive(true);
        }
    }

    private void UpdateTimer()
    {
        if (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;
        }

        // Format the time as "X:XX"
        int minutes = Mathf.FloorToInt(_timeRemaining / 60);
        int seconds = Mathf.FloorToInt(_timeRemaining % 60);
        timer.text = string.Format("{0}:{1:00}", minutes, seconds);
    }
    
    public void FoodSpawnReduceCondition()
    {
        currentCondition -= 1;
    }

    public string GetGameStatus()
    {
        if (isPaused)
        {
            return "Stop";
        }
        else if (currentCondition <= 0 || _timeRemaining <= 0)
        {
            return "End";
        }
        else
        {
            return "Go";
        }
    }
}