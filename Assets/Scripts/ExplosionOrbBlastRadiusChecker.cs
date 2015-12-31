using UnityEngine;
using System.Collections;

public class ExplosionOrbBlastRadiusChecker : MonoBehaviour {

    public GameObject explosionOrb;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //other.transform.position = new Vector3(0, 0);
        explosionOrb.GetComponent<ExplosionOrbController>().addPushTarget(other.gameObject);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        //other.transform.position = new Vector3(0, 0);
        explosionOrb.GetComponent<ExplosionOrbController>().removePushTarget(other.gameObject);
    }
}
