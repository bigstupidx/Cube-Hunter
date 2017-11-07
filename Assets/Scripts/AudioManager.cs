using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;
    public AudioClip mainMenuLoop;
    public AudioClip gameLoop;
    public AudioClip gameOverLoop;
    public AudioClip storeLoop;
    public AudioSource loopSource;
    public AudioSource fxSource;
    public bool audioStatus;

    //public int loopIndex = 0;

    public int mainMenuIndex = 0;
    public int gameIndex = 1;
    public int gameOverIndex = 2;
    public int storeIndex = 3;

  
    
	void Awake () {

        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        loopSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        SetClipInLoopSource(mainMenuIndex);

        fxSource = gameObject.AddComponent<AudioSource>() as AudioSource;

        PlaySource(loopSource);

        //audioStatus = DataManager.Instance.audioStatus;
    }

	void Update () {

        if (audioStatus)
        {
            PlaySource(loopSource);
        }
        else
        {
            if (loopSource.isPlaying)
            {
                loopSource.Stop();
                fxSource.Stop();
            }
        }
    }

    public void SetClipInLoopSource(int loopIndex)
    {
        if (loopIndex == mainMenuIndex)
        {
            loopSource.clip = mainMenuLoop;
        }
        else if (loopIndex == gameIndex)
        {
            loopSource.clip = gameLoop;
        }
        else if (loopIndex == gameOverIndex)
        {
            loopSource.clip = gameOverLoop;
        }
        else if (loopIndex == storeIndex)
        {
            loopSource.clip = storeLoop;
        }

        //PlaySource(loopSource);

    }

    public void PauseSource(AudioSource source)
    {
        source.Pause();
        audioStatus = false;
    }

    public void PlaySource(AudioSource source)
    {
        audioStatus = true;
        if (!source.isPlaying)
        {
            source.Play();
        }

    }

    public void StopSource(AudioSource source)
    {
        source.Stop();
        audioStatus = false;
    }

}
