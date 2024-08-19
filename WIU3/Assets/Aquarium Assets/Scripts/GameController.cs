using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Image conditionBar;

    [SerializeField] TextMeshProUGUI bannerText;
    [SerializeField] GameObject MakeScreenDarkerPanel;
    [SerializeField] GameObject EndGameUI;

    [SerializeField] private float _timeRemaining = 300; // Game time = 300 seconds
    [SerializeField] public float maxCondition = 200f;
    GameObject[] Seaweeds = null;
    GameObject[] smallFishes = null;
    GameObject[] bigFishes = null;

    private float _timeElapsed = 0;
    public float currentCondition;
    int SeaweedCount = 0;
    int smallFishesCount = 0;
    int bigFishesCount = 0;

    private void Start()
    {
        GameStatus("Start");
        currentCondition = maxCondition;   
    }

    void Update()
    {
        GameStatus("Go");
        GetCounts();
        EventTriggers();
        UpdateConditionBar();
        _timeElapsed += Time.deltaTime;

        // Print the counts
        //Debug.Log($"Floating Plants: {floatingPlantCount}");
        //Debug.Log($"Small Fishes: {smallFishesCount}");
        //Debug.Log($"Big Fishes: {bigFishesCount}");
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
        if (SeaweedCount <= 0) // Too less Seaweed
        {
            DecayCondition(10);
        } 
        if (smallFishesCount > 20) // Too many Small Fishes
        {
            DecayCondition(10);
        }
        if (bigFishesCount > 5) // Too many Big Fishes
        {
            DecayCondition(10);
        }

        // Lose Condition
        if (currentCondition <= 0)
        {
            // Call end screen
            bannerText.text = "Better luck next time!";
            GameStatus("Stop");
            GameStatus("End");
        }

        // Timer end Trigger
        if (_timeRemaining > 0)
        {
            if (_timeElapsed >= 1)
            {
                _timeRemaining -= 1;
                //timer.text = _timeRemaining.ToString();
                _timeElapsed = 0;
            }
        }
        else if (_timeRemaining <= 0)
        {
            // Call end screen
            bannerText.text = "Well Done!";
            GameStatus("Stop");
            GameStatus("End");
        }
    }

    private void DecayCondition(float reduceAmt) // minus this amount of condition continously
    {
        // code to reduce currentCondition overtime
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
            // initialize stuff (again)?
            // reload the scene? This State might not be needed
        }
        else if (gameStatus == "Go")
        {
            Time.timeScale = 1;
        }
        else if (gameStatus == "Stop")
        {
            Time.timeScale = 0;
            // Stop music
        }
        else if (gameStatus == "End")
        {
            // Show Endgame Screen
            MakeScreenDarkerPanel.SetActive(true);
            EndGameUI.SetActive(true);
        }
    }
}
