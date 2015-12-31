using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float range = 15;
    public float baseRange = 15;
    public int maxAirPorts = 3;

    public int teleportXP = 0;
    public int txpLevelUpRequirement = 1;
    public int lastLevel = 1;

    public GameObject teleportStreak;

    private int airPorts = 0;
    //public int forceBuildUp = 0;
    //public int isGroundedCount = 0;
    private Rigidbody2D rb2d;
    ArrayList grounds;

	// Use this for initialization
	void Start () {
        grounds = new ArrayList();
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //forceBuildUp++;
        }
        else if (Input.GetMouseButtonUp(0)) { 
            teleport();

            //Camera.main.GetComponent<Camera>().orthographicSize += 0.1f;

            //Vector3 campos = Camera.current.transform.position;
            //Camera main = GameObject.FindGameObjectWithTag("Main Camera");
            //transform.position = Input.mousePosition;// - new Vector3(Screen.width/2, Screen.height/2);
            //new Vector3(Camera.current.pixelWidth / 2, Camera.current.pixelHeight / 2);
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        //isGroundedCount++;
        grounds.Add(coll.collider.gameObject);
        airPorts = 0;
        setRange(baseRange);
    }
    void OnCollisionExit2D(Collision2D coll)
    {
        grounds.Remove(coll.collider.gameObject);
        //isGroundedCount--;
    }

    void teleport()
    {
        Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = new Vector3(click.x, click.y);
        if (Vector3.Distance(newPos, transform.position) <= range)
        {
            Vector3 oldPos = transform.position;
            //
            //Pre-Check for Collision
            //2015-12-30: still doesn't seem to work properly :/
            //
            Vector3[] dirs = new Vector3[]
            {
                Vector3.up,
                Vector3.down,
                Vector3.left,
                Vector3.right,
                new Vector3(1,1),
                new Vector3(-1,1),
                new Vector3(1,-1),
                new Vector3(-1,-1),

                new Vector3(1,.5f),
                new Vector3(-1,.5f),
                new Vector3(1,-.5f),
                new Vector3(-1,-.5f),
                new Vector3(.5f,1),
                new Vector3(-.5f,1),
                new Vector3(.5f,-1),
                new Vector3(-.5f,-1),
            };
            bool canTeleport = true;
            foreach (Vector3 dir in dirs)
            {
                Ray ray = new Ray(newPos, dir);
                if (Physics.Raycast(ray, 200.0f))
                {
                    canTeleport = false;
                    break;
                }
            }
            //
            //
            //
            if (canTeleport)
            {
                transform.position = newPos;
                showStreak(oldPos, newPos);
                //rb2d.MovePosition(newPos);
                grounds.Clear();
                teleportXP++;
                if (teleportXP == txpLevelUpRequirement)
                {
                    int lls = txpLevelUpRequirement;
                    txpLevelUpRequirement += txpLevelUpRequirement - lastLevel + 1;
                    lastLevel = lls;
                    baseRange += 0.1f;
                    setRange(baseRange);
                }
            }
            //if (forceBuildUp > 10)
            //{
            //    foreach(GameObject obj in grounds)
            //    {
            //        ExplosionOrbController.addExplosionForce(obj.GetComponent<Rigidbody2D>(), forceBuildUp*10, transform.position, forceBuildUp);
            //    }
            //}
        }
        //if (isGroundedCount <= 0)
        if (grounds.Count <= 0)
        {
            airPorts++;
            if (airPorts > maxAirPorts)
            {
                setRange(5);
            }
        }
    }

    void showStreak(Vector3 oldp, Vector3 newp)
    {
        GameObject newTS = (GameObject)Instantiate(teleportStreak);
        newTS.GetComponent<TeleportStreakUpdater>().start = oldp;
        newTS.GetComponent<TeleportStreakUpdater>().end = newp;
        newTS.GetComponent<TeleportStreakUpdater>().position();
        newTS.GetComponent<TeleportStreakUpdater>().turnOn(true);
    }

    void setRange(float newRange)
    {
        range = newRange;
        TeleportRangeIndicatorUpdater tri = GetComponentInChildren<TeleportRangeIndicatorUpdater>();
        tri.updateRange();
    }

    //Checks to make sure everything in the grounds array is still touching it
    //void checkGrounds()
    //{
    //    foreach (GameObject obj in grounds)
    //    {
    //        if obj.collider2D.rigidbody2D.
    //    }
    //}
}
