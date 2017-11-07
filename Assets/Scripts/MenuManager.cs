using Assets.Scripts;
using Assets.Scripts.GooglePlayServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(IAP))]
public class MenuManager : MonoBehaviour , IAPListener {

    public static MenuManager instance;
    public Image _audio;
    public Text highScore;
    public Sprite audioOn;
    public Sprite audioOff;
    public Image removeAdsButton;
    public Image howToPlay;
    private static IAP iap;
    private bool audioStatus;
    private static bool isInited = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        iap = GetComponent<IAP>();
        AudioManager.Instance.SetClipInLoopSource(AudioManager.Instance.mainMenuIndex);
        if (!isInited)
        {
            if (GPGS.Activate())
            {
                isInited = true;
            }
        }

        if (!DataManager.instance.isAdsActive)
        {
            removeAdsButton.gameObject.SetActive(false);
        }

        audioStatus = DataManager.instance.audioStatus;
        ChangeAudioIcon();
        highScore.text = DataManager.instance.highScore.ToString();
        Time.timeScale = 1;
    }

  
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Store()
    {
        SceneManager.LoadScene(2);
    }

    public void Scoreboard()
    {
        GPGS.ShowScoreboardUI();
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
            AudioManager.Instance.PlaySource(AudioManager.Instance.loopSource);
            _audio.sprite = audioOn;
        }
        else
        {
            AudioManager.Instance.StopSource(AudioManager.Instance.loopSource);
            _audio.sprite = audioOff;
        }
    }

    public void RemoveAds()
    {
        iap.BuyProduct(GPGSIds.REMOVE_ADS);
    }
    
    public void OpenHowToPlay()
    {
        howToPlay.gameObject.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlay.gameObject.SetActive(false);
    }

    public void PurchaseStatus(string productId, bool success)
    {
        if (success)
        {
            DataManager.instance.isAdsActive = false;
            PlayerPrefs2.SetBool(DataKeys.isAdsActiveKey, false);
            removeAdsButton.gameObject.SetActive(false);
        }
    }
}
