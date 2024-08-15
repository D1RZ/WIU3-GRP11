using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;
using TMPro;
using UnityEditor.UI;
using UnityEngine;

public class RecyclingGameManager : MonoBehaviour
{
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
    }

    public void addScore()
    {
        score += addScoreAmount;
    }

    public void minusScore()
    {
        score -= minusScoreAmount;
    }
}
