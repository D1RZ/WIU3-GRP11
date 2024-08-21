using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    private float SetDuration;
    public float duration = 60f; // Set the countdown duration in seconds
    public float timeRemaining;
    public TextMeshProUGUI timerText; // Reference to the UI Text component
    public Image Bar;
    [SerializeField] private AudioClip CountdownClip;
    [SerializeField] AudioSettingsManager audioSettingsManager;
    [SerializeField] CropSoundManager cropSoundManager;
    [SerializeField] private RandomSpawn Spawner;
    [SerializeField] private GameState gameState;
    [SerializeField] GameObject Star1;
    [SerializeField] GameObject Star2;
    [SerializeField] GameObject Star3;

    bool Star1AnimFinished = false;

    bool Star2AnimFinished = false;

    bool Star3AnimFinished = false;

    float animTimeElapsed = 0;

    float AnimCooldownTimer = 0.5f;

    void Start()
    {
        // Initialize the timer with the specified duration
        SetDuration = duration;
        timeRemaining = duration;
        UpdateTimerText();
    }

    void Update()
    {
        if (gameState.GameStart)
        {
            // Check if there is time remaining
            if (timeRemaining > 0)
            {
                // Decrease the remaining time
                timeRemaining -= Time.deltaTime;

                // Clamp the time to avoid negative values
                timeRemaining = Mathf.Max(0, timeRemaining);

                // Update the displayed timer text
                Bar.fillAmount = Mathf.Clamp(timeRemaining / duration, 0, 1);
                UpdateTimerText();
            }
            if (timeRemaining == 3)
                cropSoundManager.PlaySoundFXClip(CountdownClip, transform, audioSettingsManager.GetSFX());
            if (timeRemaining == 0 && SetDuration <= 0)
            {
                // Optional: Actions to take when the countdown reaches zero
                TimerEnded();
            }
        }
    }

    void UpdateTimerText()
    {
        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Format the timer text as MM:SS
        timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    void TimerEnded()
    {
        if (AnimCooldownTimer > 0)
            AnimCooldownTimer -= Time.deltaTime;

        // Actions to perform when the timer ends
        if ((float)Spawner.KillCount / Spawner.Bacterials.Count * 100 <= 50)
        {
            if (!Star1AnimFinished && AnimCooldownTimer <= 0) // 1 stars
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
        }
        else if((float)Spawner.KillCount / Spawner.Bacterials.Count * 100 <= 75)
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
        else
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
        Spawner.EndGame();
        Debug.Log("Countdown finished!");
        // You can add more actions here, such as stopping the game or triggering an event
    }

    public void Replay()
    {
        duration = SetDuration;
        // Initialize the timer with the specified duration
        timeRemaining = duration;
        UpdateTimerText();
        foreach (var bacterial in Spawner.Bacterials)
        {
            Destroy(bacterial.gameObject);
        }
        Spawner.StartGame();
    }
}
