using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] GameObject Star1;
    [SerializeField] GameObject Star2;
    [SerializeField] GameObject Star3;

    [SerializeField] GameObject InGameBGM;
    [SerializeField] GameObject VictoryBGM;

    private AudioSettingsManager audioSettingsManager;

    bool Star1AnimFinished = false;

    bool Star2AnimFinished = false;

    bool Star3AnimFinished = false;

    float animTimeElapsed = 0;

    float AnimCooldownTimer = 0.5f;

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
            Time.timeScale = 1;
        }
        else if (gameStatus == "Stop")
        {
            Time.timeScale = 0;
        }
        else if (gameStatus == "End")
        {  // -------------------------------------------------------------------------------------
            Time.timeScale = 1f;

            InGameBGM.SetActive(false);
            VictoryBGM.SetActive(true);

            // Show Endgame Screen
            MakeScreenDarkerPanel.SetActive(true);
            EndGameUI.SetActive(true);

            if (AnimCooldownTimer > 0)
                AnimCooldownTimer -= Time.deltaTime;

            // Check score and give stars
            if (_timeRemaining <= 0 && !Star1AnimFinished && AnimCooldownTimer <= 0) // 1 stars
            {
                Star1.SetActive(true);

                animTimeElapsed += Time.deltaTime;

                if (animTimeElapsed < 1)
                {
                    Star1.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(0, 0), new Vector2(100, 100), animTimeElapsed / 1);
                }
                else
                {
                    Star1AnimFinished = true;
                    animTimeElapsed = 0;
                    AnimCooldownTimer = 1;
                }
                Debug.Log("gimme 1 star");
            }
            else if (_timeRemaining > 20 && _timeRemaining <= 60) // 2 stars
            {
                if (!Star1AnimFinished && AnimCooldownTimer <= 0)
                {
                    Star1.SetActive(true);

                    animTimeElapsed += Time.deltaTime;

                    if (animTimeElapsed < 1)
                    {
                        Star1.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(0, 0), new Vector2(100, 100), animTimeElapsed / 1);
                    }
                    else
                    {
                        Star1AnimFinished = true;
                        animTimeElapsed = 0;
                        AnimCooldownTimer = 1;
                    }
                }
                else if (Star1AnimFinished && !Star2AnimFinished && AnimCooldownTimer <= 0)
                {
                    Star2.SetActive(true);

                    animTimeElapsed += Time.deltaTime;

                    if (animTimeElapsed < 1)
                    {
                        Star2.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(0, 0), new Vector2(100, 100), animTimeElapsed / 1);
                    }
                    else
                    {
                        Star2AnimFinished = true;
                        animTimeElapsed = 0;
                        AnimCooldownTimer = 1;
                    }
                }
            }
            else if (_timeRemaining <= 20) // 3 stars
            {
                if (!Star1AnimFinished && AnimCooldownTimer <= 0)
                {
                    Star1.SetActive(true);

                    animTimeElapsed += Time.deltaTime;

                    if (animTimeElapsed < 1)
                    {
                        Star1.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(0, 0), new Vector2(100, 100), animTimeElapsed / 1);
                    }
                    else
                    {
                        Star1AnimFinished = true;
                        animTimeElapsed = 0;
                        AnimCooldownTimer = 1;
                    }
                }
                else if (Star1AnimFinished && !Star2AnimFinished && AnimCooldownTimer <= 0)
                {
                    Star2.SetActive(true);

                    animTimeElapsed += Time.deltaTime;

                    if (animTimeElapsed < 1)
                    {
                        Star2.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(0, 0), new Vector2(100, 100), animTimeElapsed / 1);
                    }
                    else
                    {
                        Star2AnimFinished = true;
                        animTimeElapsed = 0;
                        AnimCooldownTimer = 1;
                    }
                }
                else if (Star2AnimFinished && !Star3AnimFinished && AnimCooldownTimer <= 0)
                {
                    Star3.SetActive(true);

                    animTimeElapsed += Time.deltaTime;

                    if (animTimeElapsed < 1)
                    {
                        Star3.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(0, 0), new Vector2(100, 100), animTimeElapsed / 1);
                    }
                    else
                    {
                        Star3AnimFinished = true;
                        animTimeElapsed = 0;
                    }
                }
            }
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
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}