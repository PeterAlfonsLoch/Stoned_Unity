using UnityEngine;
using System.Collections;

public class MilestoneActivatorRange : MonoBehaviour
{//2016-03-17: copied from MilestoneActivator

    public int incrementAmount = 1;

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
            playerObject.GetComponent<PlayerController>().baseRange += incrementAmount;
            Destroy(this);//makes sure it can only be used once
        }
    }
}
