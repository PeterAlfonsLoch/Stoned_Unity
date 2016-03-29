using UnityEngine;
using System.Collections;

public class PlayerAbility : MonoBehaviour {

    GameObject player;

	// Use this for initialization
	void Start () {
        player = gameObject;
	}

    public bool effectsGroundCheck()
    {
        return false;
    }

    public bool effectsAirPorts()
    {
        return false;
    }

    public bool takesGesture()
    {
        return false;
    }

    public bool takesHoldGesture()
    {
        return true;
    }

    public void processHoldGesture(Vector3 pos, float holdTime)
    {

    }

}
