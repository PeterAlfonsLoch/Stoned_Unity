using UnityEngine;
using System.Collections;

public class BoundsChecker : MonoBehaviour {

	//// Use this for initialization
	//void Start () {
	
	//}
	
	//// Update is called once per frame
	//void Update () {
	
	//}

    void OnTriggerExit2D(Collider2D coll)
    {
        coll.gameObject.transform.position = new Vector3(0, 0, coll.gameObject.transform.position.z);
    }
}
