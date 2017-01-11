using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitChecker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (true || coll.gameObject.tag == "Player")
        {
            Application.Quit();
        }
    }
}
