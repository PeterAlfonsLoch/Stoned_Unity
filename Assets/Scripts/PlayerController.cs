using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float range = 3;
    public float baseRange = 3;
    public float exhaustRange = 1;
    public int maxAirPorts = 0;
    public float exhaustCoolDownTime = 0.5f;//the cool down time for teleporting while exhausted in seconds
    public float teleportTime = 0f;//the earliest time that Merky can teleport
    public float gravityImmuneTime = 0f;//Merky is immune to gravity until this time
    public float gravityImmuneTimeAmount = 0.2f;//amount of time Merky is immune to gravity after landing (in seconds)
    private int giveGravityImmunityDelayCounter = -1;//used to delay granting gravity immunity until the next cycle
    public int gGIDCinit = 2;//note: this may go away once the teleport lookahead detector is improved

    public GameObject teleportStreak;
    public GameObject teleportStar;
    public bool useStreak = false;
    public bool useStar = true;

    public int airPorts = 0;
    private bool grounded = true;//set in isGrounded()
    private Rigidbody2D rb2d;
    private Vector2 savedVelocity;
    private float savedAngularVelocity;
    private bool velocityNeedsReloaded = false;//because you can't set a Vector2 to null, using this to see when the velocity needs reloaded
    private Vector3 gravityVector = new Vector3(0, 0);//the direction of gravity pull (for calculating grounded state)
    private Vector3 sideVector = new Vector3(0, 0);//the direction perpendicular to the gravity direction (for calculating grounded state)

    private bool inCheckPoint = false;//whether or not the player is inside a checkpoint


    public AudioClip teleportSound;

    private CameraController mainCamCtr;//the camera controller for the main camera
    private GestureManager gm;

    private ForceTeleportAbility fta;
    private ShieldBubbleAbility sba;

    //Vector3[] dirs = new Vector3[]
    //        {//for checking if Merky is grounded
    //            //Vector3.up,
    //            Vector3.down,
    //            //Vector3.left,
    //            //Vector3.right,
    //            //new Vector3(1,1),
    //            //new Vector3(-1,1),
    //            new Vector3(0.75f,-1),
    //            new Vector3(-0.75f,-1),

    //            //new Vector3(1,.5f),
    //            //new Vector3(-1,.5f),
    //            //new Vector3(1,-.5f),
    //            //new Vector3(-1,-.5f),
    //            //new Vector3(.5f,1),
    //            //new Vector3(-.5f,1),
    //            //new Vector3(.5f,-1),
    //            //new Vector3(-.5f,-1),
    //        };

    Vector3[] checkDirs = new Vector3[]
                {//for checking area around teleport target point
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

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        mainCamCtr = Camera.main.GetComponent<CameraController>();
        gm = GameObject.FindGameObjectWithTag("GestureManager").GetComponent<GestureManager>();
        fta = GetComponent<ForceTeleportAbility>();
        sba = GetComponent<ShieldBubbleAbility>();
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        Vector2 pos2 = new Vector2(pos.x, pos.y);
        //foreach (Vector3 dir in dirs)
        //{
        //    Vector2 dir2 = new Vector2(dir.x, dir.y);
        //    float length = 1.3f;
        //    dir2 = dir2.normalized * length;
        //    Vector2 start = (pos2 + dir2);
        //    Debug.DrawLine(pos2, start, Color.black);
        //}
        bool wasInAir = !grounded;
        checkGroundedState(false);
        if (wasInAir && grounded)//just landed on something
        {
            giveGravityImmunityDelayCounter = gGIDCinit;
        }
        if (giveGravityImmunityDelayCounter == 0 && grounded)
        {
            giveGravityImmunityDelayCounter = -1;
            gravityImmuneTime = Time.time + gravityImmuneTimeAmount;
            savedVelocity = rb2d.velocity;
            savedAngularVelocity = rb2d.angularVelocity;
            //Debug.Log("Just Landed" + savedVelocity);
            rb2d.isKinematic = true;
            velocityNeedsReloaded = true;
        }
        else if (giveGravityImmunityDelayCounter > 0)
        {
            giveGravityImmunityDelayCounter--;
        }
        if (gravityImmuneTime > Time.time)
        {
        }
        else {
            rb2d.isKinematic = false;
            if (velocityNeedsReloaded)
            {
                //Debug.Log("Immunity over1 "+savedVelocity);
                rb2d.velocity = savedVelocity;
                rb2d.angularVelocity = savedAngularVelocity;
                //Debug.Log("Immunity over2 " + rb2d.velocity);
                velocityNeedsReloaded = false;
            }
        }
        if (grounded && !rb2d.isKinematic && rb2d.velocity.magnitude < 0.1f)
        {
            mainCamCtr.discardMovementDelay();
        }
    }

    //void OnCollisionEnter2D(Collision2D coll)
    //{
    //    //airPorts = 0;
    //    //setRange(baseRange);
    //}
    //void OnCollisionExit2D(Collision2D coll)
    //{
    //}

    private bool teleport(Vector3 targetPos)//targetPos is in world coordinations (NOT UI coordinates)
    {
        return teleport(targetPos, true);
    }
    private bool teleport(Vector3 targetPos, bool playSound)//targetPos is in world coordinations (NOT UI coordinates)
    {
        if (teleportTime <= Time.time)
        {
            if (!isGrounded())
            {
                airPorts++;
            }
            if (airPorts > maxAirPorts)
            {
                teleportTime = Time.time + exhaustCoolDownTime;
            }
            //Get new position
            Vector3 newPos = targetPos;

            //Actually Teleport
            Vector3 oldPos = transform.position;
            transform.position = newPos;
            showTeleportEffect(oldPos, newPos);
            if (playSound)
            {
                AudioSource.PlayClipAtPoint(teleportSound, oldPos);
            }
            //Momentum Dampening
            if (rb2d.velocity.magnitude > 0.001f)//if Merky is moving
            {
                Vector3 direction = newPos - oldPos;
                float newX = rb2d.velocity.x;//the new x velocity
                float newY = rb2d.velocity.y;
                if (Mathf.Sign(rb2d.velocity.x) != Mathf.Sign(direction.x))
                {
                    newX = rb2d.velocity.x + direction.x;
                    if (Mathf.Sign(rb2d.velocity.x) != Mathf.Sign(newX))
                    {//keep from exploiting boost in opposite direction
                        newX = 0;
                    }
                }
                if (Mathf.Sign(rb2d.velocity.y) != Mathf.Sign(direction.y))
                {
                    newY = rb2d.velocity.y + direction.y;
                    if (Mathf.Sign(rb2d.velocity.y) != Mathf.Sign(newY))
                    {//keep from exploiting boost in opposite direction
                        newY = 0;
                    }
                }
                rb2d.velocity = new Vector2(newX, newY);
            }
            //Gravity Immunity
            grounded = false;
            velocityNeedsReloaded = false;//discards previous velocity if was in gravity immunity bubble
            gravityImmuneTime = 0f;
            //if (!isGrounded())//have to call it again because state has changed
            //{
            mainCamCtr.delayMovement(0.3f);
            //}
            checkGroundedState(true);
            return true;
        }
        return false;
    }

    Vector3 findTeleportablePosition(Vector3 targetPos)
    {
        Vector3 newPos = targetPos;
        //Determine if new position is in range
        Vector3 oldPos = transform.position;
        if (Vector3.Distance(newPos, transform.position) <= range
            || (GestureManager.CHEATS_ALLOWED && gm.cheatsEnabled))//allow unlimited range while cheat is active
        {
        }
        else
        {
            if (range >= baseRange)
            {
                if (Vector3.Distance(newPos, transform.position) <= range + 2)
                {
                }
            }
            else //teleporting under confinements, such as used up the airports
            {
            }
            newPos = ((newPos - oldPos).normalized * range) + oldPos;
        }

        //Determine if you can even teleport to the position (i.e. is it occupied or not?)
        {
            if (isOccupied(newPos))//test the current newPos first
            {
                //Back-tracking
                Vector3 btNewPos = newPos;
                float distance = Vector3.Distance(oldPos, newPos);
                int pointsToTry = 10;//default to trying 10 points along the line at first
                float difference = -1 * 1.00f / pointsToTry;//how much the previous jump was different by
                float percent = 1.00f;
                bool keepTrying = true;
                Vector3 norm = (newPos - oldPos).normalized;
                while (keepTrying)
                {
                    percent += difference;//actually subtraction in usual case, b/c "difference" is usually negative
                    Vector3 testPos = (norm * distance * percent) + oldPos;
                    if (isOccupied(testPos))
                    {
                    }
                    else
                    {
                        //found an open spot (tho it might not be optimal)
                        keepTrying = false;
                        btNewPos = testPos;
                    }
                }

                //Try a cardinal direction
                //Figure out which cardinal direction is closest to the one they're trying to go to: up, down, left, or right
                //whichever difference is less, is the one that's closer
                Vector3 cdNewPos = newPos;
                if (Mathf.Abs(oldPos.x - newPos.x) < Mathf.Abs(oldPos.y - newPos.y))
                {//it is closer in x direction, go up or down
                    if (oldPos.y > newPos.y)
                    {//go down
                        cdNewPos = oldPos + Vector3.down * distance;
                    }
                    else if (oldPos.y < newPos.y)
                    {//go up
                        cdNewPos = oldPos + Vector3.up * distance;
                    }
                }
                else if (Mathf.Abs(oldPos.x - newPos.x) >= Mathf.Abs(oldPos.y - newPos.y))//default: left or right
                {//it is closer in y direction, go left or right
                    if (oldPos.x > newPos.x)
                    {//go left
                        cdNewPos = oldPos + Vector3.left * distance;
                    }
                    else if (oldPos.x < newPos.x)
                    {//go right
                        cdNewPos = oldPos + Vector3.right * distance;
                    }
                }
                bool btOcc = isOccupied(btNewPos);
                bool cdOcc = isOccupied(cdNewPos);
                if (btOcc && !cdOcc)
                {
                    newPos = cdNewPos;
                }
                else if (!btOcc && cdOcc)
                {
                    newPos = btNewPos;
                }
                else if (btOcc && cdOcc)
                {
                    return oldPos;//the back up plan failed, just return, can't teleport
                }
                else if (!btOcc && !cdOcc)
                {
                    //Whichever new pos is closer to the original new pos is the winner
                    float btDist = Vector3.Distance(newPos, btNewPos);
                    float cdDist = Vector3.Distance(newPos, cdNewPos);
                    if (cdDist < btDist)
                    {
                        newPos = cdNewPos;
                    }
                    else //default to btNewPos
                    {
                        newPos = btNewPos;
                    }
                }
                else
                {
                    //ERROR! It should not be able to come here!
                }
            }
        }
        return newPos;
    }

    void showTeleportEffect(Vector3 oldp, Vector3 newp)
    {
        if (useStreak)
        {
            showStreak(oldp, newp);
        }
        if (useStar)
        {
            showTeleportStar(oldp, newp);
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
    void showTeleportStar(Vector3 oldp, Vector3 newp)
    {
        GameObject newTS = (GameObject)Instantiate(teleportStar);
        newTS.GetComponent<TeleportStarUpdater>().start = oldp;
        newTS.GetComponent<TeleportStarUpdater>().end = newp;
        newTS.GetComponent<TeleportStarUpdater>().position();
        newTS.GetComponent<TeleportStarUpdater>().turnOn(true);
    }

    void setRange(float newRange)
    {
        range = newRange;
        TeleportRangeIndicatorUpdater tri = GetComponentInChildren<TeleportRangeIndicatorUpdater>();
        tri.updateRange();
    }

    public void setGravityVector(Vector2 gravity)
    {
        if (gravity.x != gravityVector.x || gravity.y != gravityVector.y)
        {
            gravityVector = gravity;
            //v = P2 - P1    //2016-01-10: copied from an answer by cjdev: http://answers.unity3d.com/questions/564166/how-to-find-perpendicular-line-in-2d.html
            //P3 = (-v.y, v.x) / Sqrt(v.x ^ 2 + v.y ^ 2) * h
            sideVector = new Vector3(-gravityVector.y, gravityVector.x) / Mathf.Sqrt(gravityVector.x * gravityVector.x + gravityVector.y * gravityVector.y);
        }
    }

    void checkGroundedState(bool exhaust)
    {
        if (isGrounded())
        {
            airPorts = 0;
            setRange(baseRange);
        }
        else {
            if (exhaust && airPorts >= maxAirPorts)
            {
                setRange(exhaustRange);
            }
        }

    }

    bool isGrounded()
    {
        bool isGrounded = false;

        Vector3 pos = transform.position;
        Vector2 pos2 = new Vector2(pos.x, pos.y);
        int numberOfLines = 5;
        Bounds bounds = GetComponent<PolygonCollider2D>().bounds;
        float width = bounds.max.x - bounds.min.x;
        float increment = width / (numberOfLines - 1);//-1 because the last one doesn't take up any space
        float negativeOffset = increment * (numberOfLines - 1) / 2;
        Vector3 startV = bounds.min;
        float length = 0.75f;
        for (int i = 0; i < numberOfLines; i++)
        {
            Vector3 dir2 = -gravityVector.normalized * length;
            Vector3 start = pos + (sideVector.normalized * ((i * increment) - negativeOffset)) - dir2;
            Debug.DrawLine(start, start + dir2, Color.black);
            RaycastHit2D rch2d = Physics2D.Raycast(start, dir2, length);// -1*(start), 1f);
            if (rch2d && rch2d.collider != null)
            {
                GameObject ground = rch2d.collider.gameObject;
                if (ground != null && !ground.Equals(transform.gameObject))
                {
                    isGrounded = true;
                    break;
                }
            }
        }
        grounded = isGrounded;
        return isGrounded;
    }

    /**
    * Determines whether the given position is occupied or not
    */
    bool isOccupied(Vector3 pos)
    {
        Vector2 pos2 = new Vector2(pos.x, pos.y);
        foreach (Vector3 checkDir in checkDirs)
        {
            Vector2 dir2 = new Vector2(checkDir.x, checkDir.y);
            float length = 0.4f;// 1.7f;
            dir2 = dir2.normalized * length;
            Vector2 start = (pos2 + dir2);
            //Debug.DrawLine(pos2, start, Color.black, 1);
            RaycastHit2D rch2d = Physics2D.Raycast(start, -1 * dir2, length);// -1*(start), 1f);
            if (rch2d)
            {
                if (rch2d.collider != null)
                {
                    if (!rch2d.collider.isTrigger)
                    {
                        GameObject ground = rch2d.collider.gameObject;
                        if (ground != null && !ground.Equals(transform.gameObject))
                        {
                            //test opposite direction
                            start = (pos2 + -1 * dir2);
                            rch2d = Physics2D.Raycast(start, dir2, length);
                            Debug.DrawLine(start, start + dir2, Color.black, 1);
                            if (rch2d && rch2d.collider != null)
                            {
                                ground = rch2d.collider.gameObject;
                                if (ground != null && !ground.Equals(transform.gameObject))
                                {
                                    return true;//yep, it's occupied on both sides
                                }
                                //nope, it's occupied on one side but not the other
                            }
                        }
                    }
                }
                else
                {
                    GameObject go = rch2d.transform.gameObject;
                    if (go.tag.Equals("HidableArea") || go.transform.parent.gameObject.tag.Equals("HideableArea"))
                    {
                        return true;//yep, it's occupied by a hidden area
                    }
                }
            }
        }
        return false;//nope, it's not occupied
    }

    public void setIsInCheckPoint(bool iicp)
    {
        inCheckPoint = iicp;
        rb2d.isKinematic = iicp;
    }
    public bool getIsInCheckPoint()
    {
        return inCheckPoint;
    }

    public void processTapGesture(Vector3 gpos)
    {
        Vector3 newPos = findTeleportablePosition(gpos);
        teleport(newPos);
    }
    public void processTapGesture(GameObject checkPoint)
    {
        CheckPointChecker cpc = checkPoint.GetComponent<CheckPointChecker>();
        if (cpc != null)
        {
            Vector3 newPos = checkPoint.transform.position;
            teleport(newPos);
            mainCamCtr.refocus();
            cpc.trigger();
        }
    }


    public void processHoldGesture(Vector3 gpos, float holdTime, bool finished)
    {
        //if (fta.enabled)//2017-01-17 TEMP OMMISSION
        //{
        //    Vector3 newPos = findTeleportablePosition(gpos);
        //    if (finished)
        //    {
        //        if (teleport(newPos,false))
        //        {
        //            fta.processHoldGesture(newPos, holdTime, finished);
        //        }
        //    }
        //    else {
        //        fta.processHoldGesture(newPos, holdTime, finished);
        //    }
        //}
        if (sba.enabled)//2017-01-17: copied from fta section above
        {
            sba.processHoldGesture(gpos, holdTime, finished);
        }
    }
}

   
