using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimple : MonoBehaviour {

    public float speed = 1.0f;//units per second
    public float healsPerSecond = 5.0f;

    private Vector2 direction = Vector2.left;
    private float maxSpeedReached = 0;//highest speed reached since starting in this direction
    private bool healing = false;

    private Rigidbody2D rb2d;
    private HardMaterial hm;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        hm = GetComponent<HardMaterial>();
	}
	
	// Update is called once per frame
	void Update () {
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
            hm.addIntegrity(healsPerSecond*Time.deltaTime);
            if (hm.getIntegrity() == hm.maxIntegrity)
            {
                healing = false;
            }
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (rb2d.velocity.magnitude < maxSpeedReached/2)
        {
            switchDirection();
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<Rigidbody2D>() == null)
        {
            switchDirection();
        }
    }

    void switchDirection()
    {
        maxSpeedReached = 0;
        direction *= -1;
    }


}
