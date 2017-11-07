using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    public Image pBar;
    public Text pBarText;
    public float interval = 0;
    public bool isGameOver = false;
    public float pBarIncrementCount = 10;
    private float pBarIncrementValue;
    private float scoreIncrementValue;
    private float startTime;
    private float barScore = 0;
    private bool isActive = true;

    private void Start()
    {
       
        pBarIncrementValue = 1 / pBarIncrementCount;
        pBar.fillAmount = 0;
        pBarText.text = "0";
       
    }

    private void Update()
    {
        
        if (isGameOver && isActive)
        {
            Time.timeScale = 1f;
            if (ScoreManager.Score < pBarIncrementCount)
            {
                scoreIncrementValue = 1;
                pBarIncrementValue = 1 / (float)ScoreManager.Score;
                Debug.Log(scoreIncrementValue + " --- " + pBarIncrementValue);
            }
            else
            {
                scoreIncrementValue = ScoreManager.Score / pBarIncrementCount;
                pBarIncrementValue= 1 / pBarIncrementCount;
            }

            if (Time.time - startTime >= interval)
            {
                pBar.fillAmount += pBarIncrementValue;
                barScore += scoreIncrementValue;
                barScore = Mathf.Clamp(barScore, 0, ScoreManager.Score);
                if (barScore == ScoreManager.Score)
                {
                    isActive = false;
                }
                pBarText.text = ((int)barScore).ToString();
                startTime = Time.time;
            }
        }
        else
        {
            isGameOver = Camera.main.GetComponent<GameManager>().isGameOver;
            startTime = Time.time;
        }
    }
    
}
