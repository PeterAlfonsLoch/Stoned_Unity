using UnityEngine;
using System.Collections;

public class CheckPointChecker : MonoBehaviour {

    public bool activated = false;
    private GameObject ghost;
    public GameObject ghostPrefab;
    private GameObject player;

	// Use this for initialization
	void Start () {
        ghost = (GameObject)Instantiate(ghostPrefab);
        ghost.GetComponent<CheckPointGhostChecker>().sourceCP = this.gameObject;
        ghost.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
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
        ghost.SetActive(false);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Checkpoint_Root"))
        {
            if (!go.Equals(this.gameObject))
            {
                go.GetComponent<CheckPointChecker>().showRelativeTo(this.gameObject);
            }
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        bool foundCPGhost = false;//whether the player is colliding with a checkpoint ghost
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Checkpoint_Root"))
        {
            if (!go.Equals(this.gameObject))
            {
                foundCPGhost = go.GetComponent<CheckPointChecker>().checkGhostActivation();
                if (foundCPGhost)
                {
                    break;
                }
            }
        }
        if (!foundCPGhost)
        {
            activated = true;
            clearPostTeleport(true);
        }
    }

    /**
    * Displays this checkpoint relative to currentCheckpoint
    */
    public void showRelativeTo(GameObject currentCheckpoint)
    {
        if (activated)
        {
            ghost.SetActive(true);
            ghost.transform.position = currentCheckpoint.transform.position + new Vector3(2,1,0);
        }

    }
    /**
    * Checks to see if the player is colliding with this checkpoint's ghost
    */
    public bool checkGhostActivation()
    {
        return ghost.GetComponent<BoxCollider2D>().bounds.Intersects(player.GetComponent<PolygonCollider2D>().bounds);
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
