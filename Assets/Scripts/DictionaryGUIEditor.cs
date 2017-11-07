using UnityEngine;
using Assets.Scripts;

public class DictionaryGUIEditor {
#if UNITY_EDITOR

    XMLEditor xmlEditor = new XMLEditor();
    static bool isDictionaryEditor = false;
#endif

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Dictionary/Editor %k")]
    private static void DictionaryEditor()
    {

        UnityEditor.EditorWindow.GetWindowWithRect( typeof(DictionaryGUIEditor),new Rect(100,100,285,175) , true,"Dictionary Editor" );
        isDictionaryEditor = true;

    }
#endif
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Dictionary/New Language %l")]
    private static void LanguageEditor()
    {
        UnityEditor.EditorWindow.GetWindowWithRect(typeof(DictionaryGUIEditor), new Rect(100, 100, 285, 100), true, "Language Editor");
    }
#endif
#if UNITY_EDITOR
    string key, value, lang;
    public static UnityEditor.EditorGUI text;

    private void OnGUI()
    {
        if (isDictionaryEditor)
        {
            UnityEditor.EditorGUILayout.Space();
            lang = UnityEditor.EditorGUILayout.TextField("Language : ", lang);
            UnityEditor.EditorGUILayout.Space();
            key = UnityEditor.EditorGUILayout.TextField("Key : ", key);
            UnityEditor.EditorGUILayout.Space();
            value = UnityEditor.EditorGUILayout.TextField("Value : ", value);
            UnityEditor.EditorGUILayout.Space();



            if (GUI.Button(new Rect(10, 100, 75, 30), "Add"))
            {
                xmlEditor.Write(lang, key, value);
                key = value = "";
            }

            if (GUI.Button(new Rect(105, 100, 75, 30), "Update"))
            {
                xmlEditor.Update(lang, key, value);
                key = value = "";
            }

            if (GUI.Button(new Rect(200, 100, 75, 30), "Delete"))
            {
                xmlEditor.Remove(lang, key);
                key = value = "";
            }
        }
        else  // Language Editor
        {
            UnityEditor.EditorGUILayout.Space();
            UnityEditor.EditorGUILayout.Space();
            lang = UnityEditor.EditorGUILayout.TextField("Language Code: ", lang);

            if (GUI.Button(new Rect(10, 50, 260, 30), "Add"))
            {
                xmlEditor.AddNewLang(lang);
                lang = "";
            }
        }

    }
#endif
}