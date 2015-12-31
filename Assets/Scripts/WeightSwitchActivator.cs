using UnityEngine;
using System.Collections;

public class WeightSwitchActivator : MonoBehaviour {

    public bool pressed = false;
    public GameObject weightObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        Vector3 otherPos = other.collider.gameObject.transform.position;
        if (weightObject == null && otherPos.y > transform.position.y && Mathf.Abs(otherPos.x - transform.position.x) < 10)
        {
            pressed = true;
            weightObject = other.collider.gameObject;
            //other.gameObject.transform.position = new Vector3(0, 0);
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        //Vector3 otherPos = other.collider.gameObject.transform.position;
        if (other.collider.gameObject.Equals(weightObject))
        {
            pressed = false;
            weightObject = null;
            //other.gameObject.transform.position = new Vector3(0, 0);
        }
    }
}
