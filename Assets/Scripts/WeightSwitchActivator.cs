using UnityEngine;
using System.Collections;

public class WeightSwitchActivator : MonoBehaviour {

    public bool pressed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	void FixedUpdate () {
        pressed = false;
        //Determine if anything is in the weight switch's trigger area
        BoxCollider2D bc2d = GetComponentInChildren<BoxCollider2D>();
        if (bc2d != null)
        {
            Collider2D[] hitColliders = Physics2D.OverlapAreaAll(bc2d.bounds.min, bc2d.bounds.max);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                GameObject hc = hitColliders[i].gameObject;
                if (!hc.Equals(gameObject) && !hc.Equals(bc2d.transform.gameObject))
                {
                    if (hc.GetComponent<Rigidbody2D>() != null)
                    {
                        Debug.Log("Weight switched: " + hc);
                        pressed = true;
                        break;
                    }
                }
            }
        }
    }
    
}
