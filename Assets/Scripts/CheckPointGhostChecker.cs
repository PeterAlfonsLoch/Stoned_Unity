using UnityEngine;
using System.Collections;

public class CheckPointGhostChecker : MonoBehaviour {

    public GameObject sourceCP;//the checkpoint that this teleports to

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (coll.gameObject.Equals(player) && player.GetComponent<PlayerController>().getIsInCheckPoint())
        {
            coll.gameObject.transform.position = sourceCP.transform.position;
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Checkpoint_Root"))
            {
                go.GetComponent<CheckPointChecker>().showRelativeTo(this.gameObject);
            }
        }
    }
}
