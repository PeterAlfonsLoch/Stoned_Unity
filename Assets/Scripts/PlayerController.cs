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
    private Rigidbody2D rb2d;
    ArrayList grounds;

    private bool isTeleportGesture;

    public AudioClip teleportSound;

    // Use this for initialization
    void Start () {
        grounds = new ArrayList();
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
        grounds.Add(coll.collider.gameObject);
        airPorts = 0;
        setRange(baseRange);
    }
    void OnCollisionExit2D(Collision2D coll)
    {
        grounds.Remove(coll.collider.gameObject);
    }

    void teleport(bool mouseInput)
    {
        Vector3 click;
        if (mouseInput) {
            click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            click = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }

        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(click.x, click.y);
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
            grounds.Clear();
            teleportXP += 1 + bonusTXP;
            if (teleportXP >= txpLevelUpRequirement)
            {
                int lls = txpLevelUpRequirement;
                txpLevelUpRequirement += txpLevelUpRequirement - lastLevel + 1;
                lastLevel = lls;
                baseRange += 0.1f;
                setRange(baseRange);
            }        
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
}
