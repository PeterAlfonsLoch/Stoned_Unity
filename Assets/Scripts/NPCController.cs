﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{

    public AudioClip greeting;
    AudioSource source;
    
    bool greetingPlayed = false;

    private GameObject playerObject;

    // Use this for initialization
    protected virtual void Start()
    {
        source = GetComponent<AudioSource>();
        playerObject = GameManager.getPlayerObject();
    }

    // Update is called once per frame
    void Update()
    {
        source.transform.position = transform.position;        
        //Debug.Log("Number things found: " + thingsFound);
        if (canGreet())
        {
            if (!greetingPlayed || (greetingPlayed && !greetOnlyOnce()))
            {
                if (!source.isPlaying)
                {
                    source.Play();
                    //AudioSource.PlayClipAtPoint(greeting, transform.position);
                    greetingPlayed = true;
                }
            }
        }
        else
        {
            if (shouldStop())
            {
                source.Stop();
            }
        }
    }

    /// <summary>
    /// Whether or not this NPC should only greet once
    /// </summary>
    /// <returns></returns>
    protected virtual bool greetOnlyOnce()
    {
        return true;
    }

    /// <summary>
    /// Returns whether or not this NPC can play its greeting voiceline
    /// </summary>
    /// <returns></returns>
    protected virtual bool canGreet()
    {
        float distance = Vector3.Distance(playerObject.transform.position, transform.position);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, playerObject.transform.position - transform.position, distance);
        int thingsFound = hits.Length;
        return distance < 5 && thingsFound == 2;
    }

    protected virtual bool shouldStop()
    {
        return false;
    }
}
