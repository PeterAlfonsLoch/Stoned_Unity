using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour {

    public float fadeTime = 2.0f;//how long it should take to fade in or out
    private float fadeSpeed;//how fast it fades in or out

    private AudioSource music;

	// Use this for initialization
	void Start () {
        music = GetComponent<AudioSource>();
        music.volume = 0;
	}

    // Update is called once per frame
    void Update()
    {
        if (music.isPlaying)
        {
            if (music.volume > 0 && fadeSpeed < 0
                || music.volume < 1 && fadeSpeed > 0)
            {
                music.volume += fadeSpeed * Time.deltaTime;
            }
            if (music.volume <= 0)
            {
                music.Stop();
                fadeSpeed = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            fadeSpeed = calculateFadeSpeed();
            music.Play();
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            fadeSpeed = -calculateFadeSpeed();
        }
    }

    float calculateFadeSpeed()
    {
        return 1.0f / fadeTime;
    }
}
