using UnityEngine;
using System.Collections;

public class FloatCubeController : MonoBehaviour {

    public GameObject switchObj;
    public float maxHeight;
    public float propulsionHeight;//how far off the ground the float cube can go
    public float lift = 50;
    public GameObject psgoTrail;
    public GameObject psgoSparks;

    private Rigidbody2D rb;
    public float liftForce;//the amount of force to use for lift
    public float levelLiftForce;//the amount of force needed to keep the float cube level
    public float velocityThreshold = 3;//the max allowed velocity while floating
    public float variance;//the amount of variance in either direction the propulsionHeight is allowed to be
    private Vector3 upVector;
    //private Vector3 stableVector;
    private bool increasingLastTime = false;
    private float initAngDrag;
    private Vector3 upDirection;//used to determine the up direction of the float cube
    private Quaternion upAngle;//used to determine which direction the float cube should rotate towards
    //Particles
    private ParticleSystem psTrail;
    private ParticleSystem psSparks;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        initAngDrag = rb.angularDrag;
        upVector = new Vector2(0, lift);
        liftForce = 0;
        levelLiftForce = 0;
        if (propulsionHeight <= 0)
        {
            propulsionHeight = maxHeight - transform.position.y + GetComponent<SpriteRenderer>().bounds.extents.y -(variance/2);//to facilitate setting the propulsionHeight
        }
        psTrail = psgoTrail.GetComponent<ParticleSystem>();
        if (psTrail != null)
        {
            psTrail.Pause();
            psTrail.Clear();
        }
        psSparks = psgoSparks.GetComponent<ParticleSystem>();
        if (psSparks != null)
        {
            psSparks.Pause();
            psSparks.Clear();
        }
        upDirection = transform.up;
        upAngle = Quaternion.Euler(transform.eulerAngles);
        //Debug.Log("diff: " + maxHeight + "-" + transform.position.y + "= " + (maxHeight - transform.position.y));
        //stableVector = new Vector2(0, 90);
    }

    // Update is called once per frame
    void Update()
    {
        //if (propulsionHeight <= 0)
        //{
        //    propulsionHeight = maxHeight - transform.position.y;//to facilitate setting the propulsionHeight
        //    Debug.Log("diff "+gameObject.name+": " + maxHeight + "-" + transform.position.y + "= " + (maxHeight - transform.position.y));
        //}
        if (switchObj.GetComponent<WeightSwitchActivator>().pressed)
        {
            if (psTrail != null)
            {
                psTrail.Play();
            }
            if (psSparks != null)
            {
                psSparks.Play();
            }
            if (propulsionHeight > 0)
            {//use new system
                rb.angularDrag = initAngDrag;
                rb.freezeRotation = true;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, upAngle, 5);
                //float velX = rb.velocity.x;
                //if (velX > 0)
                //{
                //    rb.velocity += new Vector2(-1, 0);
                //}
                //else if (velX < 0)
                //{
                //    rb.velocity += new Vector2(1, 0);
                //}
                bool propping1 = false;
                bool propping2 = false;

                Vector2 start = getGroundVector(propulsionHeight);
                Debug.DrawLine(start, transform.position, Color.black);
                RaycastHit2D rch2d = Physics2D.Raycast(start, Vector2.up, propulsionHeight);
                if (rch2d && rch2d.collider != null)
                {
                    GameObject ground = rch2d.collider.gameObject;
                    if (ground != null && !ground.Equals(transform.gameObject))
                    {
                        propping1 = true;
                    }
                }
                start = getGroundVector(-(propulsionHeight + variance));
                rch2d = Physics2D.Raycast(start, Vector2.up, propulsionHeight + variance);
                if (rch2d && rch2d.collider != null)
                {
                    GameObject ground = rch2d.collider.gameObject;
                    if (ground != null && !ground.Equals(transform.gameObject))
                    {
                        propping2 = true;
                    }
                }
                if (propping1 && propping2)
                {
                    if (rb.gravityScale > -0.5f)
                    {
                        rb.gravityScale = -0.5f;
                    }
                    if (rb.velocity.y <= 0)
                    {
                        rb.gravityScale--;
                        if (rb.velocity.y > velocityThreshold)
                        {
                            rb.velocity = new Vector2(rb.velocity.x, velocityThreshold);
                            rb.gravityScale++;
                        }
                    }
                    //if (rb.velocity.y == 0)
                    //{
                    //    liftForce++;
                    //}
                    //else if (rb.velocity.y <= 0)
                    //{
                    //    liftForce += -rb.velocity.y;
                    //}
                    //else if (rb.velocity.y > velocityThreshold)
                    //{
                    //    liftForce -= (rb.velocity.y - velocityThreshold);
                    //}
                    //else
                    //{
                    //}
                }
                else if (!propping1 && propping2)
                {
                    rb.gravityScale = 0;
                    lockAxes(rb, false, true);
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    //rb.isKinematic = true;
                    //liftForce -= rb.velocity.y;
                }
                else
                {
                    rb.gravityScale = 1;
                    if (rb.velocity.y > 0)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                    }
                    lockAxes(rb, false, false);
                    //rb.velocity = Vector2.zero;
                    //liftForce = 0;
                }
                //if (liftForce < 0)
                //{
                //    liftForce = 0;
                //}
                //else if (liftForce > 0)
                //{
                //    upVector = new Vector2(0, liftForce);
                //    rb.AddForce(upVector);
                //}
            }
            else {//use old system (will be removed in the future)
                rb.angularDrag = initAngDrag;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 1);
                if (transform.position.y < maxHeight)// && rb.velocity.magnitude < 100)
                {
                    rb.AddForce(upVector);
                    if (rb.gravityScale > 0)
                    {
                        rb.gravityScale -= 0.1f;
                    }
                    if (rb.gravityScale < 0)
                    {
                        rb.gravityScale = 0;
                    }
                    //Vector3.MoveTowards(transform.position, new Vector3(0, 20), Time.deltaTime * 5);
                    increasingLastTime = true;
                }
                else
                {
                    if (increasingLastTime)
                    {
                        rb.velocity = Vector2.zero;
                        increasingLastTime = false;
                    }
                    if (transform.position.y > maxHeight)
                    {
                        rb.gravityScale += 0.1f;
                    }
                    //rb.gravityScale = 0.5;
                    //rb.isKinematic = true;
                    //Vector3.RotateTowards(transform.rotation, Quaternion.identity,10,0.0f);
                    //rb.AddForce(stableVector);

                }
            }
        }
        else {
            if (psTrail != null)
            {
                psTrail.Pause();
                psTrail.Clear();
            }
            if (psSparks != null)
            {
                psSparks.Pause();
                psSparks.Clear();
            }
            rb.angularDrag = 0.05f;
            rb.gravityScale = 1;
            rb.freezeRotation = false;
            lockAxes(rb, false, false);
            //rb.isKinematic = false;
        }
    }

    Vector3 getGroundVector(float propulsionHeight)//returns a vector with magnitude propulsionHeight and angle this.upDirection
    {
        return transform.position - upDirection.normalized * propulsionHeight;
    }

    /**
    *Sets either the x or y axis lock to the freeze value
    */
    void lockAxes(Rigidbody2D rb2d, bool x, bool freeze)
    {
        if (x)
        {
            if (freeze)
            {
                if (rb2d.constraints == RigidbodyConstraints2D.FreezePositionY)
                {
                    rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                }
                else
                {
                    rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
                }
            }
            else
            {
                if (rb2d.constraints == RigidbodyConstraints2D.FreezeAll)
                {
                    rb2d.constraints = RigidbodyConstraints2D.FreezePositionY;
                }
                else
                {
                    rb2d.constraints = RigidbodyConstraints2D.None;
                }
            }
        }
        else
        {
            if (freeze)
            {
                if (rb2d.constraints == RigidbodyConstraints2D.FreezePositionX)
                {
                    rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                }
                else
                {
                    rb2d.constraints = RigidbodyConstraints2D.FreezePositionY;
                }
            }
            else
            {
                if (rb2d.constraints == RigidbodyConstraints2D.FreezeAll)
                {
                    rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
                }
                else
                {
                    rb2d.constraints = RigidbodyConstraints2D.None;
                }
            }
        }
        if (freeze) { rb2d.constraints = RigidbodyConstraints2D.FreezeAll; }
        else { rb2d.constraints = RigidbodyConstraints2D.None; }
    }

}
