using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Image conditionBar;

    [SerializeField] public float maxCondition = 200f;
    GameObject[] floatingPlants = null;
    GameObject[] smallFishes = null;
    GameObject[] bigFishes = null;

    public float currentCondition;
    int floatingPlantCount = 0;
    int smallFishesCount = 0;
    int bigFishesCount = 0;

    private void Start()
    {
        GameStatus("Start");
        currentCondition = maxCondition;   
    }

    void Update()
    {
        
        GameStatus("Ongoing");
        GetCounts();
        EventTriggers();
        UpdateConditionBar();

        // Print the counts
        //Debug.Log($"Floating Plants: {floatingPlantCount}");
        //Debug.Log($"Small Fishes: {smallFishesCount}");
        //Debug.Log($"Big Fishes: {bigFishesCount}");
    }

    private void GetCounts()
    {
        // Find all active objects
        floatingPlants = GameObject.FindGameObjectsWithTag("FloatingPlant");
        smallFishes = GameObject.FindGameObjectsWithTag("SmallFish");
        bigFishes = GameObject.FindGameObjectsWithTag("BigFish");

        // Get the counts of each type
        floatingPlantCount = floatingPlants.Length;
        smallFishesCount = smallFishes.Length;
        bigFishesCount = bigFishes.Length;
    }

    private void EventTriggers()
    {
        // Decay Condition Triggers
        if (floatingPlantCount > 10) // Too many Floating Plants
        {
            DecayCondition(10);
            Debug.Log("Condition decrease due to too many floating plants");
        } 
        if (smallFishesCount > 15) // Too many Small Fishes
        {
            DecayCondition(10);
            Debug.Log("Condition decrease due to too many small fishes");
        }
        if (bigFishesCount > 5) // Too many Big Fishes
        {
            DecayCondition(10);
            Debug.Log("Condition decrease due to too many big fishes");
        }

        // Lose Condition
        if (currentCondition <= 0)
        {
            GameStatus("End");
        }

        // Timer end Trigger
        // add code to check time and switch gameStatus to "End"
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
            Debug.Log("Game Start");
            // initialize stuff (again)?
        }
        else if (gameStatus == "Ongoing")
        {
            Debug.Log("Game Ongoing");
            // Do nothing
        }
        else if (gameStatus == "End")
        {
            Debug.Log("Game End");
            // End gametimer
            // Reset gametimer?
            // Show Endgame Screen
        }
    }
}
