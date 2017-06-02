using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{

    private PolygonCollider2D coll;
    public float gravityScale = 9.81f;
    public bool mainGravityZone = true;//true to change camera angle, false to not
    private Vector2 gravityVector;
    private List<Rigidbody2D> tenants = new List<Rigidbody2D>();//the list of colliders in this zone

    // Use this for initialization
    void Start()
    {
        coll = GetComponent<PolygonCollider2D>();
        gravityVector = -transform.up.normalized * gravityScale;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.isTrigger)
        {
            Rigidbody2D rb2d = coll.gameObject.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                tenants.Add(rb2d);
            }
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (!coll.isTrigger)
        {
            Rigidbody2D rb2d = coll.gameObject.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                tenants.Remove(coll.gameObject.GetComponent<Rigidbody2D>());
            }
        }
    }
    void FixedUpdate()
    {
        foreach (Rigidbody2D rb2d in tenants)
        {
            {
            Vector3 vector = gravityVector * rb2d.mass;
            rb2d.AddForce(vector);
            //Inform the gravity accepters
            if (mainGravityZone)
            {
                GravityAccepter ga = rb2d.gameObject.GetComponent<GravityAccepter>();
                if (ga != null)
                {
                    ga.Gravity = this.gravityVector;
                }
            }
        }
        //Check to see if the camera rotation needs updated
        if (mainGravityZone && Camera.main.transform.rotation != transform.rotation)
        {
            //Check to see if Merky is in this GravityZone
            if (GameManager.getPlayerObject().GetComponent<GravityAccepter>().Gravity == gravityVector)
            {
                Camera.main.GetComponent<CameraController>().setRotation(transform.rotation);
            }
        }
    }
}
