using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts;

public class GameDictionary : MonoBehaviour {

    public static string language;
    public static Dictionary<string, string> translations;
    private XMLEditor xmlEditor;
    

    private void Awake()
    {
        xmlEditor = new XMLEditor();
        xmlEditor.Read();
    }

    private void Start()
    {
        language = DataManager.instance.language;
        translations = xmlEditor.GetLang(language);
    }

    public static string Translate(string gObjectId)
    {
        if (translations.ContainsKey(gObjectId))
        {
            return translations[gObjectId];
        }
        else
            return "Çeviri Hatası";
    }
    
}
