using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimple : MonoBehaviour
{

    public float speed = 1.0f;//units per second
    public float healsPerSecond = 5.0f;

    private Vector2 direction = Vector2.left;
    private float maxSpeedReached = 0;//highest speed reached since starting in this direction
    private bool healing = false;

    private Rigidbody2D rb2d;
    private HardMaterial hm;
    private GameObject player;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        hm = GetComponent<HardMaterial>();
        direction = Utility.PerpendicularLeft(transform.up).normalized;
        player = GameManager.getPlayerObject();
    }

    // Update is called once per frame
    void Update()
    {
        float tempSpeed = speed;
        if (rb2d.velocity.magnitude < speed / 4)
        {
            tempSpeed *= speed * 2;
        }
        if (hm.getIntegrity() < hm.maxIntegrity / 4 || healing)
        {
            healing = true;
            tempSpeed = 0;
        }
        if (tempSpeed > 0)
        {
            rb2d.AddForce(rb2d.mass * direction * tempSpeed);
        }
        if (rb2d.velocity.magnitude > maxSpeedReached)
        {
            maxSpeedReached = rb2d.velocity.magnitude;
        }
        if (rb2d.velocity.magnitude < 0.1f)
        {
            switchDirection();
            hm.addIntegrity(healsPerSecond * Time.deltaTime);
            if (hm.getIntegrity() == hm.maxIntegrity)
            {
                healing = false;
            }
        }
        if (senseInFront() == null //there's a cliff up ahead
            && !Utility.lineOfSight(gameObject, player) //nothing between it and the player
            )
        {
            switchDirection();
            rb2d.AddForce(rb2d.mass * direction * speed * 4);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (rb2d.velocity.magnitude < maxSpeedReached / 2)
        {
            switchDirection();
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.isTrigger && coll.gameObject.GetComponent<Rigidbody2D>() == null)
        {
            switchDirection();
        }
    }

    void switchDirection()
    {
        maxSpeedReached = 0;
        direction *= -1;
    }

    GameObject senseInFront()
    {
        Vector2 ahead = direction * 2;
        Vector2 senseDir = ahead - (Vector2)transform.up.normalized;
        Debug.DrawLine((Vector2)transform.position + ahead, senseDir + (Vector2)transform.position, Color.blue);
        RaycastHit2D rch2d = Physics2D.Raycast((Vector2)transform.position + ahead, -transform.up, 1);
        if (rch2d)
        {
            return rch2d.collider.gameObject;
        }
        return null;
    }
}
