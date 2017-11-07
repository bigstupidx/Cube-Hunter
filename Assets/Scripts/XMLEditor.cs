using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml;
using System.IO;

namespace Assets.Scripts
{
    class XMLEditor
    {
        public static string path  = Application.dataPath + "/Languages.xml";
        public Dictionary<string, Dictionary<string, string>> languages = new Dictionary<string, Dictionary<string, string>>();
        public XmlDocument document = new XmlDocument();
        public XmlNodeList rootChilds;


       
        private void OpenDictionary()
        {
            document.Load(path);
            rootChilds = document.DocumentElement.ChildNodes;
        }

        public void Read()
        {
            OpenDictionary();
            foreach (XmlNode lang in rootChilds)
            {
                Dictionary<string, string> words = new Dictionary<string, string>();
                foreach (XmlNode word in lang.ChildNodes)
                {
                    words.Add(word["key"].InnerText, word["value"].InnerText);
                }

                languages.Add(lang.Attributes[0].Value, words);
            }
            
        }

        public void Write(string language, string key, string value)
        {
            OpenDictionary();
            foreach (XmlNode lang in rootChilds)
            {
                if (lang.Attributes[0].Value == language)
                {

                    XmlElement _word = document.CreateElement("word");

                    XmlElement _key = document.CreateElement("key");
                    _key.InnerText = key;
                    XmlElement _value = document.CreateElement("value");
                    _value.InnerText = value;

                    _word.AppendChild(_key);
                    _word.AppendChild(_value);

                    lang.AppendChild(_word);

                    document.Save(path);
                    break;
                }
            }
        }

        public void Update(string language, string key, string value)
        {
            OpenDictionary();
            foreach (XmlNode lang in rootChilds)
            {
                if (lang.Attributes[0].Value == language)
                {
                    foreach (XmlNode word in lang.ChildNodes)
                    {
                        if (word.ChildNodes[0].InnerText == key)
                        {
                            word.ChildNodes[1].InnerText = value;
                            document.Save(path);
                            return;
                        }
                    }
                }
            }
        }

        public void Remove(string language, string key)
        {
            OpenDictionary();
            foreach (XmlNode lang in rootChilds)
            {
                if (lang.Attributes[0].Value == language)
                {
                    foreach (XmlNode word in lang.ChildNodes)
                    {
                        if (word.ChildNodes[0].InnerText == key)
                        { 
                            lang.RemoveChild(word);
                            document.Save(path);
                            return;
                        }
                    }
                }
            }
        }

        public void AddNewLang(string lang)
        {
            OpenDictionary();
            XmlElement _lang = document.CreateElement("language");
            _lang.InnerText = " ";
            _lang.SetAttribute("id", lang);

            XmlElement root = document.DocumentElement ;
            root.AppendChild(_lang);

            document.Save(path);
            
        }
        
        public Dictionary<string , string> GetLang(string lang)
        {
            if (languages.ContainsKey(lang))
            {
                return languages[lang];
            }
            else
                return null;
        }

    }
}
