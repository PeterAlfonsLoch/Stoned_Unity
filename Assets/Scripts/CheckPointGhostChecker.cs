using UnityEngine;
using System.Collections;

public class CheckPointGhostChecker : MonoBehaviour {

    public GameObject sourceCP;//the checkpoint that this teleports to
    public GameObject curCP;//the checkpoint that the player is currently at

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.Equals(GameObject.FindGameObjectWithTag("Player")))
        {
            coll.gameObject.transform.position = sourceCP.transform.position;
            curCP.GetComponent<CheckPointChecker>().clearPostTeleport(true);
        }
    }
}
