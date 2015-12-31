using UnityEngine;
using System.Collections;

public class PreCheckerController : MonoBehaviour {

    public bool isColliding = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = new Vector3(click.x, click.y);
        transform.position = newPos;
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        isColliding = true;
    }
}
