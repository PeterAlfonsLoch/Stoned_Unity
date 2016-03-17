using UnityEngine;
using System.Collections;

public class MilestoneActivator : MonoBehaviour {

    private GameObject playerObject;
    private bool used = false;

    // Use this for initialization
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if ( ! used && coll.gameObject.Equals(playerObject))
        {
            used = true;
            playerObject.GetComponent<PlayerController>().maxAirPorts++;
            Destroy(this);//makes sure it can only be used once
        }
    }
}
