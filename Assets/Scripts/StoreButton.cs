
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreButton : MonoBehaviour {

    private string skinName;
    private Store store;
    private Sprite _sprite;
    public bool isUnlocked = false;


    private void Start()
    {
        store = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Store>();
        skinName = gameObject.name;
        _sprite = GetComponent<Image>().sprite;
    }

    public void Buy()
    {
        if (!isUnlocked)
        {
            store.requestedSkin = this;
            store.BuySkin(skinName);
        }
        else
        {
            CancelSelection();
            DataManager.instance.WriteData(DataKeys.skinNameKey, skinName);
            DataManager.instance.skinName = skinName;
            transform.GetChild(0).gameObject.SetActive(true);
        }

        
    }

    public void Unlock()
    {
        DataManager.instance.WriteData(skinName, true);
        int index = store.blockSpritesLocked.IndexOf(_sprite);
        _sprite = store.blockSprites[index];
        CancelSelection();
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<Image>().sprite = _sprite;
        DataManager.instance.WriteData(DataKeys.skinNameKey, skinName);
        DataManager.instance.skinName = skinName;
        isUnlocked = true;
    }

    public void CancelSelection()
    {
        for (int i = 0; i < store.buttonContentPanel.transform.childCount; i++)
        {
            if (store.buttonContentPanel.transform.GetChild(i).name == DataManager.instance.skinName)
            {
                store.buttonContentPanel.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }
    }

}
