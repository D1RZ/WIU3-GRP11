using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecyclingGameManager : MonoBehaviour
{
    public bool stopActions;

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
    [SerializeField] GameObject AudioSettingsPanel;

    [SerializeField] private string[] quotes;

    private AudioSettingsManager audioSettingsManager;

    [SerializeField] GameObject Star1;

    [SerializeField] GameObject Star2;

    [SerializeField] GameObject Star3;

    bool Star1AnimFinished = false;

    bool Star2AnimFinished = false;

    bool Star3AnimFinished = false;

    float animTimeElapsed = 0;

    float AnimCooldownTimer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        stopActions = false;
        doublePoints = false;

        score = 0;

        for (int i = 0; i < Hoops.Length; i++)
        {
            Hoops[i].GetComponent<Hoop>().moveSpeed = 0;
        }

        audioSettingsManager = AudioSettingsPanel.GetComponent<AudioSettingsManager>();

        audioSettingsManager.Load();

        AudioSettingsPanel.SetActive(false);
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
            stopActions = true;
            ChangeMusic();
            finalScore.text = "Score: " + score;
            MakeScreenDarkerPanel.SetActive(true);
            EndGameUI.SetActive(true);
            WhichQuote();
        }
        else
        {
            stopActions = false;
        }
    }

    private void WhichQuote()
    {
        if (AnimCooldownTimer > 0)
            AnimCooldownTimer -= Time.deltaTime;

        if (score < 15)
        {
            bannerText.text = quotes[0];
        }
        else if (score >= 15 && score < 35)
        {
            bannerText.text = quotes[1];

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
        }
        else if (score >= 35 && score < 65)
        {
            bannerText.text = quotes[2];

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
        else if (score >= 65)
        {
            bannerText.text = quotes[3];


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

    private void ChangeHoopsSpeed()
    {
        if (_timeRemaining < 90)
        {
            for (int i = 0; i < Hoops.Length; i++)
            {
                Hoops[i].GetComponent<Hoop>().moveSpeed = 0.2f;
            }
        }
        if (_timeRemaining < 30)
        {
            doublePoints = true;
            DoublePointsIndicator.SetActive(true);

            timer.color = Color.red;

            for (int i = 0; i < Hoops.Length; i++)
            {
                Hoops[i].GetComponent<Hoop>().moveSpeed = 0.45f;
            }
        }
    }

    private void ChangeMusic()
    {
        BGMObject.SetActive(false);

        EndSoundObject.SetActive(true);
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
