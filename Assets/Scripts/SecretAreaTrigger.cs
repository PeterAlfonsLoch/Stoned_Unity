﻿using UnityEngine;
using System.Collections;

public class SecretAreaTrigger : MonoBehaviour {

    public GameObject secretHider;
    private GameObject playerObject;

	// Use this for initialization
	void Start () {
        playerObject = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        //FUTURE CODE: check to make sure the player is the one who collided before destroying the secret hider
        if (coll.gameObject.Equals(playerObject))
        {
            GameObject.Destroy(secretHider);
        }
    }
}
