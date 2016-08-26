using UnityEngine;
using System.Collections;

public class BoundsChecker : MonoBehaviour {

    public bool active = false;//whether or not it will actually check the bounds

	//// Use this for initialization
	//void Start () {
	
	//}
	
	//// Update is called once per frame
	//void Update () {
	
	//}

    void OnTriggerExit2D(Collider2D coll)
    {
        if (active)
        {
            coll.gameObject.transform.position = new Vector3(0, 0, coll.gameObject.transform.position.z);
        }
    }
}
