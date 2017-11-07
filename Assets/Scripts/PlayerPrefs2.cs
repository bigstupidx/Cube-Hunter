using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerPrefs2 
    {
        public static void SetBool(string key , bool state)
        {
            PlayerPrefs.SetInt(key, state ? 1 : 0);
        }

        public static bool GetBool(string key)
        {
            if (PlayerPrefs.GetInt(key) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
