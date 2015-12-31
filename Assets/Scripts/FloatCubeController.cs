using UnityEngine;
using System.Collections;

public class FloatCubeController : MonoBehaviour {

    public GameObject switchObj;
    public int maxHeight;

    private Rigidbody2D rb;
    private Vector3 upVector;
    //private Vector3 stableVector;
    private bool increasingLastTime = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        upVector = new Vector2(0, 50);
        //stableVector = new Vector2(0, 90);
    }

    // Update is called once per frame
    void Update()
    {
        if (switchObj.GetComponent<WeightSwitchActivator>().pressed)
        {
            rb.angularDrag = 50f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity,1);
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
        else {
            rb.angularDrag = 0.05f;
            rb.gravityScale = 1;
            //rb.isKinematic = false;
        }
    }

}
