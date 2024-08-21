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
            //if (timeRemaining == 3)
                //cropSoundManager.PlaySoundFXClip(CountdownClip, transform, audioSettingsManager.GetSFX());
            else
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
        // Actions to perform when the timer ends
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
