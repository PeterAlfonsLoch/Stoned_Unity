using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float range = 15;
    public float baseRange = 15;
    public float exhaustRange = 2.5f;
    public int maxAirPorts = 3;

    public int teleportXP = 0;
    public int txpLevelUpRequirement = 1;
    public int lastLevel = 1;

    public GameObject teleportStreak;

    public int airPorts = 0;
    private Rigidbody2D rb2d;

    private bool isTeleportGesture;

    public AudioClip teleportSound;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        Input.simulateMouseWithTouches = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount == 0)
        {
            isTeleportGesture = true;
        }
        else if (Input.touchCount >= 2)
        {
            isTeleportGesture = false;
        }
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (isTeleportGesture)//don't let the pinch zoom gesture count as a teleport gesture
                {
                    teleport(false);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
            }
            else if (Input.GetMouseButtonUp(0))
            {
                teleport(true);                
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        airPorts = 0;
        setRange(baseRange);
    }
    void OnCollisionExit2D(Collision2D coll)
    {
    }

    void teleport(bool mouseInput)
    {
        checkGroundedState(false);

        //Get new position
        Vector3 click;
        if (mouseInput) {
            click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            click = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        Vector3 newPos = new Vector3(click.x, click.y);
        //Determine if new position is in range
        Vector3 oldPos = transform.position;
        int bonusTXP = 0;
        if (Vector3.Distance(newPos, transform.position) <= range)
        {
        }
        else
        {
            if (range >= baseRange)
            {
                if (Vector3.Distance(newPos, transform.position) <= range + 2)
                {
                    bonusTXP = 1;
                }
            }
            else //teleporting under confinements, such as used up the airports
            {
                bonusTXP = -1;//don't give any txp for teleporting beyond max air ports
            }
            newPos = ((newPos - oldPos).normalized * range) + oldPos;
        }
        transform.position = newPos;
        showStreak(oldPos, newPos);
        AudioSource.PlayClipAtPoint(teleportSound, oldPos);
        //Give teleport xp
        teleportXP += 1 + bonusTXP;
        if (teleportXP >= txpLevelUpRequirement)
        {
            int lls = txpLevelUpRequirement;
            txpLevelUpRequirement += txpLevelUpRequirement - lastLevel + 1;
            lastLevel = lls;
            baseRange += 0.1f;
            //setRange(baseRange);
        }
        checkGroundedState(true);
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

    void checkGroundedState(bool update)
    {        
        if (isGrounded())
        {
            airPorts = 0;
            setRange(baseRange);
        }
        else {
            if (update)
            {
                airPorts++;
                if (airPorts > maxAirPorts)
                {
                    setRange(exhaustRange);
                }
            }
        }

    }

    bool isGrounded()
    {
        bool isGrounded = false;
        Vector3[] dirs = new Vector3[]
            {
                //Vector3.up,
                Vector3.down,
                //Vector3.left,
                //Vector3.right,
                //new Vector3(1,1),
                //new Vector3(-1,1),
                new Vector3(1,-1),
                new Vector3(-1,-1),

                //new Vector3(1,.5f),
                //new Vector3(-1,.5f),
                //new Vector3(1,-.5f),
                //new Vector3(-1,-.5f),
                //new Vector3(.5f,1),
                //new Vector3(-.5f,1),
                //new Vector3(.5f,-1),
                //new Vector3(-.5f,-1),
            };
        Vector3 pos = transform.position;
        Vector2 pos2 = new Vector2(pos.x, pos.y);
        foreach (Vector3 dir in dirs)
        {
            Vector2 dir2 = new Vector2(dir.x, dir.y);
            float length = 1.3f;
            dir2 = dir2.normalized * length;
            Vector2 start = (pos2 + dir2);
            Debug.DrawLine(pos2, start,Color.black,1);
            RaycastHit2D rch2d = Physics2D.Raycast(start, -1*dir2, length);// -1*(start), 1f);
            if (rch2d && rch2d.collider != null)
            {
                GameObject ground = rch2d.collider.gameObject;
                if (ground != null && ! ground.Equals(transform.gameObject))
                {
                    isGrounded = true;
                    break;
                }
            }
        }
        return isGrounded;
    }
}
