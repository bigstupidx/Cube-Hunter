using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager Instance { get; set; }
    public Text scoreText;
    public List<int> levelAngers;
    private static int score;
    private int currentLevelIndex;


    public static int  Score { get { return score; } }

    void Start ()
    {
        Instance = this;
        score = 0;
        currentLevelIndex = 0;
	}

    private void Update()
    {
        scoreText.text = score.ToString();
        
        if (score >= levelAngers[currentLevelIndex])
        {
            if (currentLevelIndex < levelAngers.Count -1)
                currentLevelIndex++;

            switch (currentLevelIndex)
            {
                case 0:
                    {
                        SetTimeScale(1.0f);
                        break;
                    }
                case 1:
                    {
                        SetTimeScale(1.25f);
                        break;
                    }
                case 3:
                    {
                        SetTimeScale(1.5f);
                        break;
                    }
                case 4:
                    {
                        SetTimeScale(1.75f);
                        break;
                    }
                default:
                    break;
            }
        }
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void IncreaseScore()
    {
        score++;
    }
    
    public void ResetData()
    {
        score = 0;
        currentLevelIndex = 0;
    }

}
