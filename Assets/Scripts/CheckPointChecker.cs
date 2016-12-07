using UnityEngine;
using System.Collections;

public class CheckPointChecker : MemoryMonoBehaviour
{

    public static GameObject current = null;//the current checkpoint

    public bool activated = false;
    private GameObject ghost;
    public Bounds ghostBounds;
    public GameObject ghostPrefab;
    private GameObject player;
    private PlayerController plyrController;
    private static Camera checkpointCamera;

    // Use this for initialization
    void Start()
    {
        ghost = (GameObject)Instantiate(ghostPrefab);
        ghost.SetActive(false);
        ghostBounds = ghost.GetComponent<SpriteRenderer>().bounds;
        player = GameObject.FindGameObjectWithTag("Player");
        plyrController = player.GetComponent<PlayerController>();
        if (checkpointCamera == null) {
            checkpointCamera = GameObject.Find("CP BG Camera").GetComponent<Camera>();
            checkpointCamera.gameObject.SetActive(false);
        }
    }

    //When a player touches this checkpoint, activate it
    void OnCollisionEnter2D(Collision2D coll)
    {
        activate();
    }

    /**
    * When the player is inside, show the other activated checkpoints
    */
    void OnTriggerEnter2D(Collider2D coll)
    {
        trigger();
    }
    public void activate()
    {
        activated = true;
        GameManager.saveMemory(this);
        GameManager.saveCheckPoint(this);
    }
    public void trigger()
    {
        current = this.gameObject;
        activate();
        ghost.SetActive(false);
        plyrController.setIsInCheckPoint(true);
        player.transform.position = this.gameObject.transform.position;
        foreach (CheckPointChecker cpc in GameManager.getActiveCheckPoints())
        {
            if (!cpc.Equals(this))
            {
                cpc.showRelativeTo(this.gameObject);
            }
        }
        grabCheckPointCameraData();
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (current == this.gameObject)
        {
            plyrController.setIsInCheckPoint(false);
            activate();
            clearPostTeleport(true);
            current = null;
        }
    }

    void grabCheckPointCameraData()//2016-12-06: grabs image data from the camera designated for checkpoints
    {
        checkpointCamera.gameObject.SetActive(true);
        checkpointCamera.gameObject.transform.position = gameObject.transform.position + new Vector3(0,0,-10);
        //2016-12-06: The following code copied from an answer by jashan: http://answers.unity3d.com/questions/22954/how-to-save-a-picture-take-screenshot-from-a-camer.html
        int resWidth = 100;
        int resHeight = 100;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        checkpointCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        checkpointCamera.Render();        
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0,0,rt.width,rt.height), 0, 0);
        screenShot.Apply();
        checkpointCamera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        SpriteRenderer sr = ghost.GetComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), new Vector2(0.5f, 0.5f));
        checkpointCamera.gameObject.SetActive(false);
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
            foreach (CheckPointChecker cpc in GameManager.getActiveCheckPoints())
            {
                if (cpc != this)
                {
                    GameObject go = cpc.gameObject;
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

    public override MemoryObject getMemoryObject()
    {
        return new CheckPointCheckerMemory(this);
    }
}

//
//Class that saves the important variables of this class
//2016-11-26: copied from SecretAreaTrigger::SecretAreaTriggerMemory
//
public class CheckPointCheckerMemory : MemoryObject
{
    public CheckPointCheckerMemory() { }//only called by the method that reads it from the file
    public CheckPointCheckerMemory(CheckPointChecker cpc) : base(cpc)
    {
        saveState(cpc);
    }

    public override void loadState(GameObject go)
    {
        CheckPointChecker cpc = go.GetComponent<CheckPointChecker>();
        if (cpc != null)
        {
            if (this.found)
            {
                cpc.activate();
            }
        }
    }
    public override void saveState(MemoryMonoBehaviour mmb)
    {
        CheckPointChecker cpc = ((CheckPointChecker)mmb);
        this.found = cpc.activated;
    }
}

