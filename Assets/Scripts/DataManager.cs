using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts;

public class DataManager : MonoBehaviour, IReadable, IWritable {

    public List<GameObject> skinObjects;
    private List<string> skins;
    
    public static DataManager instance;
    [NonSerialized]
    public bool audioStatus;
    [NonSerialized]
    public string skinName;
    [NonSerialized]
    public int highScore;
    [NonSerialized]
    public bool isAdsActive;
    [NonSerialized]
    public string language;
    [NonSerialized]
    public int lastScore;
    [NonSerialized]
    public int gold;
    [NonSerialized]
    public int point;


    private void Awake()
    {
        Debug.Log(Screen.width);
        
        skins = new List<string>();
        foreach (GameObject item in skinObjects)
        {
            skins.Add(item.name);
        }

        //PlayerPrefs.DeleteAll();

        if (!PlayerPrefs.HasKey(DataKeys.cubeHunterKey))
        {
            GameSetup();
        }
        else
        {
            CheckSkins();
        }

        ReadData();
        instance = this;
        
    }

    public void GameSetup()
    {
       WriteData();
    }

    public void CheckSkins()
    {
        foreach (string item in skins)
        {
            if (!PlayerPrefs.HasKey(item))
            {
                WriteData(item, false);
            }
        }

    }

    public void WriteData()
    {
        audioStatus = true;
        highScore = 0;
        isAdsActive = true;
        language = "EN";
        lastScore = 0;
        gold = 0;
        point = 0;
        
        skinName = skins[1];
        
        foreach (string item in skins)
        {
            Debug.Log(item);
            WriteData(item, false);
        }
        WriteData(DataKeys.cubeHunterKey, "CubeHunter");
        WriteData(DataKeys.audioStatusKey, audioStatus);
        WriteData(DataKeys.isAdsActiveKey, isAdsActive);
        WriteData(DataKeys.skinNameKey, skinName);
        WriteData(DataKeys.highScoreKey, highScore);
        WriteData(DataKeys.languageKey, language);
        WriteData(DataKeys.lastScoreKey, lastScore);
        WriteData(DataKeys.goldKey, gold);
        WriteData(DataKeys.pointKey, point);
        WriteData(skinName, true);
        WriteData(skins[0], true);
        
    }

    public void WriteData(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public void WriteData(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public void WriteData(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
    
    public void WriteData(string key, bool value)
    {
        PlayerPrefs2.SetBool(key, value);
    }

    public void ReadData()
    {
        skinName = ReadStringData(DataKeys.skinNameKey);
        audioStatus = ReadBoolData(DataKeys.audioStatusKey);
        lastScore = ReadIntData(DataKeys.lastScoreKey);
        highScore = ReadIntData(DataKeys.highScoreKey);
        isAdsActive = ReadBoolData(DataKeys.isAdsActiveKey);
        language = ReadStringData(DataKeys.languageKey);
        point = ReadIntData(DataKeys.pointKey);
        gold = ReadIntData(DataKeys.goldKey);
    }

    public string ReadStringData(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public float ReadFloatData(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }

    public int ReadIntData(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    public bool ReadBoolData(string key)
    {
        return PlayerPrefs2.GetBool(key);
    }

}
