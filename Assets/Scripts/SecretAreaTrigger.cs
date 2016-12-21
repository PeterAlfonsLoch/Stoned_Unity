﻿using UnityEngine;
using System.Collections;

public class SecretAreaTrigger : MonoBehaviour
{

    public GameObject secretHider;
    private GameObject playerObject;

    // Use this for initialization
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (secretHider == null)//if the secret hider isn't set, then it's probably its parent
        {
            secretHider = transform.parent.gameObject;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.Equals(playerObject))
        {
            HiddenArea ha;
            ha = secretHider.GetComponent<HiddenArea>();
            ha.nowDiscovered();
        }
    }
}