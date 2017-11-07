using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;
using Assets.Scripts.GooglePlayServices;

public class AdManager : MonoBehaviour
{

    public static AdManager Instance { get; set; }
    public AdManagerListener adListener;

    public string unityAndroidGameId;
    public string unityIOSGameId;
    public string unityInterstitialId;
    public string unityRewardedId;

    public string adMobAndroidBannerId;
    public string adMobAndroidInterstitialId;
    public string adMobAndroidRewardedId;
    public string adMobIOSBannerId;
    public string adMobIOSInterstitialId;
    public string adMobIOSRewardedId;
    
    private bool isAdsActive = true; 
    private BannerView bannerView;
    private InterstitialAd interstialAd;
    private RewardBasedVideoAd adMobRewardedAd; 
   
    public bool IsAdsActive
    {
        get { return isAdsActive; }
        set { isAdsActive = value; }
    }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeUnityAds();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void RequestAdMobRewardedVideo()
    {
        #if UNITY_ANDROID
            string adId = adMobAndroidRewardedId;
        #elif UNITY_IOS
                string adId = adMobIOSRewardedId;
        #endif

        adMobRewardedAd = RewardBasedVideoAd.Instance;
        AdRequest request = new AdRequest.Builder().Build();
        adMobRewardedAd.LoadAd(request, adId);
        adMobRewardedAd.OnAdRewarded += RewardedAd_OnAdRewarded;
        adMobRewardedAd.OnAdClosed += RewardedAd_OnAdClosed;
    }

    public void RequestAdMobBanner()
    {
        #if UNITY_ANDROID
            string adId = adMobAndroidBannerId;
        #elif UNITY_IOS
                string adId = adMobIOSBannerId;
        #endif

        bannerView = new BannerView(adId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }

    public void RequestAdMobInterstial()
    {
        #if UNITY_ANDROID
            string adId = adMobAndroidInterstitialId;
        #elif UNITY_IOS
                string adId = adMobIOSInterstitialId;
        #endif

        interstialAd = new InterstitialAd(adId);
        AdRequest request = new AdRequest.Builder().Build();
        interstialAd.LoadAd(request);
    }

    public void InitializeUnityAds()
    {
        #if UNITY_ANDROID
             Advertisement.Initialize(unityAndroidGameId);
        #elif UNITY_IOS
              Advertisement.Initialize(unityIOSGameId);
        #endif
    }

    public void ShowAdMobBannerAd()
    {
        if (!isAdsActive)
            return;

        bannerView.Show();
    }

    public void HideAdMobBannerAd()
    {
        bannerView.Hide();
    }

    public void ShowAdMobInterstitialAd()
    {
        if (!isAdsActive)
            return;

        if (interstialAd.IsLoaded())
        {
            interstialAd.Show();
        }
    }

    public void ShowAdMobRewardedAd()
    {
        if(adMobRewardedAd.IsLoaded())
        {
            adMobRewardedAd.Show();
        }

    }

    public void ShowUnityInterstitialAd()
    {
        if (!isAdsActive)
            return;

        if (Advertisement.IsReady(unityInterstitialId))
        {
            Advertisement.Show(unityInterstitialId);
        }
    }

    public void ShowUnityRewardedAd()
    {
        if (Advertisement.IsReady(unityRewardedId))
        {
            Advertisement.Show(unityRewardedId, new ShowOptions() { resultCallback = HandleAdResult });
        }
        else
        {
            if (adListener != null)
                adListener.AdManagerMessage("NetworkError","CHECK YOUR INTERNET CONNECTION!");
        }
    }

    public void DestroyBanner()
    {
        bannerView.Destroy();
    }
    
    private void HandleAdResult(ShowResult result)
    {
        bool isWatched = false;

        switch (result)
        {
            case ShowResult.Failed:
                isWatched = false;
                break;
            case ShowResult.Skipped:
                isWatched = false;
                break;
            case ShowResult.Finished:
                {
                    isWatched = true;
                    break;
                }
        }

        if (adListener != null)
            adListener.RewardedAdWatchedStatus(isWatched);
    }

    private void RewardedAd_OnAdClosed(object sender, System.EventArgs e)
    {
        if (adListener != null)
            adListener.RewardedAdWatchedStatus(false);
    }

    private void RewardedAd_OnAdRewarded(object sender, Reward e)
    {
        if (adListener != null)
            adListener.RewardedAdWatchedStatus(true);
    }
}
