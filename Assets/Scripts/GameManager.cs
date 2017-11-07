using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using Assets.Scripts.GooglePlayServices;

public class GameManager : MonoBehaviour , AdManagerListener{

    public static GameManager Instance { get; set; }

    public Canvas continueCanvas;
    public Canvas menuCanvas;
    public Canvas gameOverCanvas;

    [NonSerialized]
    public bool isGameOver = false;
    [NonSerialized]
    public bool isContinued = false;
    public int price = 50;
    public Image smallAudio;
    public Image bigAudio;
    public Sprite audioOn;
    public Sprite audioOff;
    public Text scoreText;
    public Text totalGoldText;
    public Text message;
    public Text highScoreMessage;
    public ushort dataCode = 0;
    private AudioManager audioManager;
    private  bool audioStatus;
    private int gold;
    private float currentTimeScale;
    

    private void Awake()
    {
        Instance = this;
        AdManager.Instance.adListener = this;
        audioManager = AudioManager.Instance;
        audioManager.SetClipInLoopSource(audioManager.gameIndex);
    }

    private void Start ()
    {
        continueCanvas.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        gold = DataManager.instance.gold;
        audioStatus = PlayerPrefs2.GetBool(DataKeys.audioStatusKey);
        ChangeAudioIcon();
    }

	private void Update () 
    {
        if (isGameOver)
        {
            if (isContinued)
            {
                if (!gameOverCanvas.gameObject.activeSelf)
                {
                    GameOver();
                }
            }
            else
            {
                Pause();
                totalGoldText.text = gold.ToString();
                continueCanvas.gameObject.SetActive(true);
                isContinued = true;
                isGameOver = false;
            }
        }
	}

    public void GameOver()
    {
        AdManager.Instance.ShowUnityInterstitialAd();
        isGameOver = true;
        audioManager.SetClipInLoopSource(audioManager.gameOverIndex);
        gameOverCanvas.gameObject.SetActive(true);
        scoreText.text = ScoreManager.Score.ToString();
        message.gameObject.SetActive(false);
        highScoreMessage.gameObject.SetActive(false);
        CheckHighScore();

        gold += ScoreManager.Score;
        DataManager.instance.WriteData(DataKeys.goldKey, gold);
        DataManager.instance.gold = gold;
    }

    public void OpenMenu()
    {
        Pause();
        menuCanvas.gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        Resume();
        menuCanvas.gameObject.SetActive(false);
    }

    public void Pause()
    {
        currentTimeScale = Time.timeScale;
        Time.timeScale = 0;
        //audioManager.PauseSource(audioManager.loopSource);
    }

    public void Resume()
    {
        Time.timeScale = currentTimeScale;
        ///audioManager.PlaySource(audioManager.loopSource);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        ScoreManager.Instance.ResetData();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void AudioOnOff()
    {
        audioStatus = !audioStatus;
        PlayerPrefs2.SetBool(DataKeys.audioStatusKey, audioStatus);
        DataManager.instance.audioStatus = audioStatus;
        ChangeAudioIcon();
    }

    private void ChangeAudioIcon()
    {
        if (audioStatus)
        {
            smallAudio.sprite = bigAudio.sprite = audioOn;
            audioManager.PlaySource(audioManager.loopSource);
        }
        else
        {
            smallAudio.sprite = bigAudio.sprite = audioOff;
            audioManager.PauseSource(audioManager.loopSource);
        }
    }

    public void ContinueWithGold()
    { 
        if (gold >= price)
        {
            gold = gold - price;
            continueCanvas.gameObject.SetActive(false);
            DataManager.instance.WriteData(DataKeys.goldKey, gold);
            DataManager.instance.gold = gold;
            Resume();
        }
        else
        {
            
            message.gameObject.SetActive(true);
            message.text = "YOU DON'T HAVE ENOUGH GOLD!";
        }
    }

    public void ContinueWithAd()
    {
        message.gameObject.SetActive(false);
        AdManager.Instance.ShowUnityRewardedAd();
    }

    public void CheckHighScore()
    {
        int highScore = DataManager.instance.highScore;
        if (ScoreManager.Score > highScore)
        {
            DataManager.instance.highScore = highScore;
            DataManager.instance.WriteData(DataKeys.highScoreKey, ScoreManager.Score);
            highScoreMessage.gameObject.SetActive(true);
            GPGS.PostScore(GPGSIds.leaderboard_scoreboard, ScoreManager.Score);
        }
        else
        {
            highScoreMessage.gameObject.SetActive(false);
        }
    }
    
    public void AdManagerMessage(string messageText)
    {
        AdManagerMessage("", messageText);    
    }
    public void AdManagerMessage(string messageKey , string messageText)
    {
        if (messageKey == "NetworkError")
        {
            message.gameObject.SetActive(true);
            message.text = messageText;
        }
        else
            Debug.Log(messageText);
    }

    public void RewardedAdWatchedStatus(bool isWatched)
    {
        if (isWatched)
        {
            continueCanvas.gameObject.SetActive(false);
            Resume();
            isContinued = true;
        }
    }
}
