using UnityEngine;
using System.Collections;

public class PlayerAbility : MonoBehaviour {

    GameObject player;
    protected PlayerController playerController;
    public Color effectColor;//the color used for the particle system upon activation

    // Use this for initialization
    protected void Start () {
        player = gameObject;
        playerController = GetComponent<PlayerController>();
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

    public void processHoldGesture(Vector3 pos, float holdTime, bool finished)
    {

    }

}
