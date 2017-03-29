using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{

    public AudioClip greeting;
    AudioSource source;

    bool triggerEntered = false;
    bool audioPlaying = false;
    float audioPlayEndTime = 0;
    bool greetingPlayed = false;

    private GameObject playerObject;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        playerObject = GameManager.getPlayerObject();
    }

    // Update is called once per frame
    void Update()
    {
        source.transform.position = transform.position;
        float distance = Vector3.Distance(playerObject.transform.position, transform.position);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, playerObject.transform.position - transform.position, distance);
        int thingsFound = hits.Length;
        //Debug.Log("Number things found: " + thingsFound);
        if (distance < 5 && thingsFound == 2)
        {
            if (!greetingPlayed)
            {
                if (!source.isPlaying)
                {
                    audioPlaying = true;
                    source.Play();
                    //AudioSource.PlayClipAtPoint(greeting, transform.position);
                    audioPlayEndTime = Time.time + greeting.length;
                    greetingPlayed = true;
                }
            }
        }
    }
}
