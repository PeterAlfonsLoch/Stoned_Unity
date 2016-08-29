using UnityEngine;
using System.Collections;

public class CheckPointChecker : MonoBehaviour {

    public bool activated = true;
    private GameObject ghost;
    public GameObject ghostPrefab;

	// Use this for initialization
	void Start () {
        ghost = (GameObject)Instantiate(ghostPrefab);
        ghost.GetComponent<CheckPointGhostChecker>().sourceCP = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //When a player touches this checkpoint, activate it
    void OnCollisionEnter2D(Collision2D coll)
    {
        activated = true;
    }

    /**
    * When the player is inside, show the other activated checkpoints
    */
    void OnTriggerEnter2D(Collider2D coll)
    {
        activated = true;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Checkpoint_Root"))
        {
            if (!go.Equals(this.gameObject))
            {
                go.GetComponent<CheckPointChecker>().showRelativeTo(this.gameObject);
            }
        }
    }
    //void OnTriggerExit2D(Collider2D coll)
    //{
    //    activated = true;
    //    clearPostTeleport(true);
    //}

    /**
    * Displays this checkpoint relative to currentCheckpoint
    */
    public void showRelativeTo(GameObject currentCheckpoint)
    {
        if (activated)
        {
            ghost.GetComponent<CheckPointGhostChecker>().curCP = currentCheckpoint;
            ghost.SetActive(true);
            ghost.transform.position = currentCheckpoint.transform.position + new Vector3(2,1,0);
        }

    }
    /**
    * So now the player has teleported and the checkpoint ghosts need to go away
    */
    public void clearPostTeleport(bool first)
    {
        ghost.SetActive(false);
        if (first)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Checkpoint_Root"))
            {
                go.GetComponent<CheckPointChecker>().clearPostTeleport(false);
            }
        }
    }
}
