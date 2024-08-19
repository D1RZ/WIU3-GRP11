using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;
using TMPro;
using UnityEditor.UI;
using UnityEngine;

public class RecyclingGameManager : MonoBehaviour
{
    [SerializeField] GameObject MakeScreenDarkerPanel;
    [SerializeField] GameObject EndGameUI;
    [SerializeField] TMP_Text bannerText;
    [SerializeField] TMP_Text finalScore;

    [SerializeField] private float _timeRemaining = 90.0f; // Game time
    private float _timeElapsed = 0.0f;
    [SerializeField] private TMP_Text timer;

    private float score;
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private float addScoreAmount;
    [SerializeField] private float minusScoreAmount;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(score);
        scoreDisplay.text = "Score: " + score;
        UpdateTimer();
        CheckGameEnded();
    }

    public void addScore()
    {
        score += addScoreAmount;
    }

    public void minusScore()
    {
        score -= minusScoreAmount;
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

    private void CheckGameEnded()
    {
        if(_timeRemaining <= 0)
        {
            Time.timeScale = 0;
            MakeScreenDarkerPanel.SetActive(true);
            EndGameUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
