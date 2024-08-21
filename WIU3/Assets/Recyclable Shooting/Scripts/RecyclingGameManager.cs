using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecyclingGameManager : MonoBehaviour
{

    [SerializeField] private GameObject BGMObject;
    [SerializeField] private GameObject EndSoundObject;

    [SerializeField] private GameObject MakeScreenDarkerPanel;
    [SerializeField] private GameObject EndGameUI;
    [SerializeField] private GameObject DoublePointsIndicator;
    [SerializeField] private TMP_Text bannerText;
    [SerializeField] private TMP_Text finalScore;

    [SerializeField] private float _timeRemaining = 90.0f; // Game time
    private float _timeElapsed = 0.0f;
    [SerializeField] private TMP_Text timer;

    [SerializeField] private GameObject[] Hoops;

    private float score;
    public bool doublePoints;
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private float addScoreAmount;
    [SerializeField] private float minusScoreAmount;

    [SerializeField] private string[] quotes;


    // Start is called before the first frame update
    void Start()
    {
        doublePoints = false;

        score = 0;

        for (int i = 0; i < Hoops.Length; i++)
        {
            Hoops[i].GetComponent<Hoop>().moveSpeed = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //score = 36;
        //Debug.Log(score);
        scoreDisplay.text = "Score: " + score;
        UpdateTimer();
        ChangeHoopsSpeed();
        CheckGameEnded();
    }

    public void addScore()
    {
        if (doublePoints)
        {
            score += addScoreAmount * 2;
        }
        else
        {
            score += addScoreAmount;
        }
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
            timer.text = string.Format("{0}:{1:00}", 0, 0);
            Time.timeScale = 0;
            WhichQuote();
            ChangeMusic();
            finalScore.text = "Score: " + score;
            MakeScreenDarkerPanel.SetActive(true);
            EndGameUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void WhichQuote()
    {
        if (score < 15)
        {
            bannerText.text = quotes[0];
        }
        else if (score >= 15 && score < 35)
        {
            bannerText.text = quotes[1];

        }
        else if (score >= 35 && score < 65)
        {
            bannerText.text = quotes[2];

        }
        else if (score >= 65)
        {
            bannerText.text = quotes[3];

        }
    }

    private void ChangeHoopsSpeed()
    {
        if (_timeRemaining < 90)
        {
            for (int i = 0; i < Hoops.Length; i++)
            {
                Hoops[i].GetComponent<Hoop>().moveSpeed = 0.3f;
            }
        }
        if (_timeRemaining < 30)
        {
            doublePoints = true;
            DoublePointsIndicator.SetActive(true);

            timer.color = Color.red;

            for (int i = 0; i < Hoops.Length; i++)
            {
                Hoops[i].GetComponent<Hoop>().moveSpeed = 0.65f;
            }
        }
    }

    private void ChangeMusic()
    {
        BGMObject.GetComponent<AudioSource>().Stop();
        BGMObject.SetActive(false);

        EndSoundObject.SetActive(true);
        EndSoundObject.GetComponent<AudioSource>().Play();
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
