using UnityEngine;
using System.Collections;

public class ExplosionOrbController : MonoBehaviour {
    public int forceAmount = 300;
    //public int pushTargets = 0;
    ArrayList pushTargets;

	// Use this for initialization
	void Start () {
        pushTargets = new ArrayList();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

     void OnTriggerEnter2D(Collider2D other)
    {
        //Vector3 otherPos = other.collider.gameObject.transform.position;// = new Vector3(0,0);
        //other.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(otherPos - transform.position  * 500);
        if (  ! pushTargets.Contains(other.gameObject))
        {
            pushTargets.Add(other.gameObject);
        }
        foreach (GameObject go in pushTargets)
        {
            //GameObject go = ((GameObject)obj);
            //go.GetComponent<Rigidbody2D>().AddForce(go.transform.position - transform.position * -forceAmount);
            
            addExplosionForce(go.GetComponent<Rigidbody2D>(), forceAmount, transform.position, 10f);
        }
        pushTargets.Clear();
    }

    public void addPushTarget(Object target)
    {
        pushTargets.Add(target);
    }

    public void removePushTarget(Object target)
    {
        pushTargets.Remove(target);
    }

    /**
    * Retrieved on 2015-12-20 from http://forum.unity3d.com/threads/need-rigidbody2d-addexplosionforce.212173/
    * Originally posted by Swamy
    */
    public static void addExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        Vector3 dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.transform.position = Vector3.MoveTowards(body.transform.position, dir.normalized*explosionRadius + body.transform.position, 100 * Time.deltaTime);// * explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }

}
