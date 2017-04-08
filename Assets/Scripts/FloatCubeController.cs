using UnityEngine;
using System.Collections;

public class FloatCubeController : MonoBehaviour
{

    public GameObject switchObj;
    public GameObject psgoTrail;
    public GameObject psgoSparks;
    public float propulsionHeight;//how far off the ground the float cube can go
    public float liftForce = 0;//how much force can be applied each frame
    public float expectedGravity = 9.81f;//the expected gravity scale for this float cube
    public float forceMultiplier = 2;//how much to multiply things by for default lift force
    public float forceMultiplierSteady = 0.5f;//how much to multiply when trying to remain steady

    private Rigidbody2D rb;
    private BoxCollider2D bc2d;
    public float variance;//the amount of variance in either direction the propulsionHeight is allowed to be
    private bool proppingLastTime = false;//whether or not it was propping last time
    private Vector3 upDirection;//used to determine the up direction of the float cube
    private Quaternion upAngle;//used to determine which direction the float cube should rotate towards
    //Particles
    private ParticleSystem psTrail;
    private ParticleSystem psSparks;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
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
        if (liftForce == 0)
        {
            liftForce = rb.mass * expectedGravity;
        }
        upDirection = transform.up.normalized;
        upAngle = Quaternion.Euler(transform.eulerAngles);
    }

    // Update is called once per frame
    void Update()
    {
        if (switchObj.GetComponent<WeightSwitchActivator>().pressed)
        {
            if (psTrail != null)
            {
                psTrail.transform.localRotation = transform.localRotation;
                psTrail.Play();
            }
            if (psSparks != null)
            {
                psSparks.Play();
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
        }
    }

    void FixedUpdate()
    {
        if (switchObj.GetComponent<WeightSwitchActivator>().pressed)
        {
            if (propulsionHeight > 0)
            {
                rb.freezeRotation = true;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, upAngle, 5);
                bool propping1 = false;
                bool propping2 = false;

                Vector2 start = getGroundVector(propulsionHeight);
                Debug.DrawLine(start, transform.position, Color.black);
                RaycastHit2D rch2d = Physics2D.Raycast(start, upDirection, propulsionHeight);
                if (rch2d && rch2d.collider != null)
                {
                    GameObject ground = rch2d.collider.gameObject;
                    if (ground != null && !ground.Equals(transform.gameObject))
                    {
                        propping1 = true;
                    }
                }
                start = getGroundVector((propulsionHeight + variance));
                rch2d = Physics2D.Raycast(start, upDirection, propulsionHeight + variance);
                if (rch2d && rch2d.collider != null)
                {
                    GameObject ground = rch2d.collider.gameObject;
                    if (ground != null && !ground.Equals(transform.gameObject))
                    {
                        propping2 = true;
                    }
                }
                //
                float currentFloatHeight = propulsionHeight;//how far above the ground it currently is
                RaycastHit2D[] rh2ds = new RaycastHit2D[10];
                bc2d.Cast(-upDirection, rh2ds, propulsionHeight, true);
                foreach (RaycastHit2D rch2dl in rh2ds)
                {
                    if (rch2dl && rch2dl.collider != null && !rch2dl.collider.isTrigger)
                    {
                        GameObject ground = rch2dl.collider.gameObject;
                        if (ground != null && !ground.Equals(transform.gameObject))
                        {
                            currentFloatHeight = rch2dl.distance;
                            break;
                        }
                    }
                }
                //
                if (propping1 && propping2)
                {
                    proppingLastTime = true;
                    float force = liftForce * forceMultiplier;
                    rb.AddForce(upDirection * force);
                }
                else if (!propping1 && propping2)
                {
                    if (proppingLastTime)
                    {
                        rb.velocity = Vector2.zero;
                        proppingLastTime = false;
                    }
                    rb.AddForce(upDirection * liftForce * forceMultiplierSteady);
                }
                else
                {
                    Debug.DrawLine(start, transform.position, Color.red);
                }
            }
        }
        else
        {
            rb.freezeRotation = false;
        }
    }

    /// <summary>
    /// Returns a vector with magnitude propulsionHeight and angle this.upDirection
    /// </summary>
    /// <param name="propulsionHeight"></param>
    /// <returns></returns>
    Vector3 getGroundVector(float propulsionHeight)
    {
        return transform.position - upDirection.normalized * propulsionHeight;
    }
}
