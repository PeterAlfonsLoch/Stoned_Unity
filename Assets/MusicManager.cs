using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages which bgm to play.
/// It takes "song requests" from multiple sources and decides which one to play
/// </summary>
public class MusicManager : MonoBehaviour {

    private AudioSource currentSong;//the current song that is playing
    private AudioSource prevSong;//the previous song that was playing

    public float fadeTime = 2.0f;//how long it should take to fade in or out
    private float fadeSpeed = 0;//how fast it fades in or out (determined by fadeTime)

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeSpeed != 0)
        {
            float shift = fadeSpeed * Time.deltaTime;
            currentSong.volume += shift;
            if (prevSong)
            {
                prevSong.volume -= shift;
                if (prevSong.volume <= 0)
                {
                    prevSong.Stop();
                    fadeSpeed = 0;
                }
            }
            else if (currentSong.volume >= 1)
            {
                fadeSpeed = 0;
            }
        }
	}

    public void setCurrentSong(AudioSource newSong)
    {
        if (newSong != currentSong)
        {
            prevSong = currentSong;
            currentSong = newSong;
            currentSong.volume = 0;
            currentSong.Play();
            fadeSpeed = 1.0f / fadeTime;
        }
    }
}
