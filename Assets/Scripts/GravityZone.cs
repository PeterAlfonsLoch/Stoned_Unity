using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour {

    private BoxCollider2D coll;
    public float gravityScale = 1f;

	// Use this for initialization
	void Start () {
        coll = GetComponent<BoxCollider2D>();
	}
	
	void FixedUpdate () {
        List<Collider2D> c2ds = new List<Collider2D>();
        c2ds = GameManager.gravityColliderList;
        //Debug.Log("Collider list: " + c2ds.Count);
        foreach (Collider2D c2d in c2ds){
            if (true || coll.bounds.Contains(c2d.bounds.center))
            {
                //Debug.Log("Found object: " + c2d.gameObject);
                //;//transform.rotation.eulerAngles
                Vector3 vector = new Vector3(-3, -4,0).normalized * gravityScale * Time.fixedDeltaTime;
                //Debug.Log("Applying vector: " + vector);
                c2d.gameObject.GetComponent<Rigidbody2D>().AddForce(vector);
            }
        }
	}
}
