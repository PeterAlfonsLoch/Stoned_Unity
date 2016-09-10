using UnityEngine;
using System.Collections;

public class CheckPointChecker : MonoBehaviour
{

    public static GameObject current = null;//the current checkpoint

    public bool activated = false;
    private GameObject ghost;
    public Bounds ghostBounds;
    public GameObject ghostPrefab;
    private GameObject player;
    private PlayerController plyrController;

    // Use this for initialization
    void Start()
    {
        ghost = (GameObject)Instantiate(ghostPrefab);
        ghost.SetActive(false);
        ghostBounds = ghost.GetComponent<SpriteRenderer>().bounds;
        player = GameObject.FindGameObjectWithTag("Player");
        plyrController = player.GetComponent<PlayerController>();
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
        trigger();
    }
    public void trigger()
    {
        current = this.gameObject;
        activated = true;
        ghost.SetActive(false);
        plyrController.setIsInCheckPoint(true);
        player.transform.position = this.gameObject.transform.position;
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
        if (current == this.gameObject)
        {
            plyrController.setIsInCheckPoint(false);
            activated = true;
            clearPostTeleport(true);
            current = null;
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
            ghost.transform.position = currentCheckpoint.transform.position + (gameObject.transform.position - currentCheckpoint.transform.position).normalized * 4;
            ghostBounds = ghost.GetComponent<SpriteRenderer>().bounds;

            //check to make sure its ghost does not intersect other CP ghosts
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Checkpoint_Root"))
            {
                if (go != gameObject)
                {
                    CheckPointChecker cpc = go.GetComponent<CheckPointChecker>();
                    if (cpc.activated && cpc.ghost.activeSelf)
                    {
                        if (ghostBounds.Intersects(cpc.ghostBounds))
                        {
                            if (Vector3.Distance(go.transform.position, current.transform.position) < Vector3.Distance(gameObject.transform.position, current.transform.position))
                            {
                                clearPostTeleport(false);
                                return;
                            }
                            else
                            {
                                cpc.clearPostTeleport(false);
                            }
                        }
                    }
                }
            }
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
    * Checks to see if this checkpoint's ghost contains the targetPos 
    */
    public bool checkGhostActivation(Vector3 targetPos)
    {
        return ghost.GetComponent<BoxCollider2D>().bounds.Contains(targetPos);
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
                if (!go.Equals(this.gameObject))
                {
                    go.GetComponent<CheckPointChecker>().clearPostTeleport(false);
                }
            }
        }
    }
}
