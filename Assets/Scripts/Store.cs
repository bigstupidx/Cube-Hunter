using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts;
using UnityEngine.UI;
using Assets.Scripts.GooglePlayServices;

[RequireComponent(typeof(IAP))]
public class Store : MonoBehaviour , IAPListener{

    public static Store instance;
    public string activeSkinName;
    public List<Sprite> blockSprites;
    public List<Sprite> blockSpritesLocked;
    public Image buttonContentPanel;
    public GameObject skinButton;
    public Text skinPriceMessage;
    public IAP iap;
    public bool isAdsActive;

    public StoreButton requestedSkin;


    void Start() {

        instance = this;
        AudioManager.Instance.SetClipInLoopSource(AudioManager.Instance.storeIndex);
        iap = GetComponent<IAP>();

        activeSkinName = DataManager.instance.skinName;

        for (int i = 0; i < blockSprites.Count; i++)
        {
            bool status = PlayerPrefs2.GetBool(blockSprites[i].name);

            GameObject gObject = Instantiate(skinButton) as GameObject;
            gObject.name = blockSprites[i].name;
            gObject.transform.SetParent(buttonContentPanel.transform, false);
           
            if (status)
            {
                gObject.GetComponent<Image>().sprite = blockSprites[i];
            }
            else
            {
                gObject.GetComponent<Image>().sprite = blockSpritesLocked[i];
            } 
            gObject.GetComponent<StoreButton>().isUnlocked = status;

            if (gObject.name == activeSkinName)
            {
                gObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                gObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        string price = "";
        iap.GetProductPrice("skins");

        if (price == null)
            skinPriceMessage.text = "Check your internet connection for skin prices.";
        else
            skinPriceMessage.text = "Each skin price is " + price;

    }

    public void BuySkin(string skinName)
    {
        iap.BuyProduct(GPGSIds.SKIN);
    } 
    
    public void Back()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void PurchaseStatus(string productId, bool success)
    {
        if (success)
        {
            requestedSkin.Unlock();
        }
    }
}
