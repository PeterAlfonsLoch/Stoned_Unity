using UnityEngine;
using System.Collections;

public class GestureProfile {

    protected GameObject player;
    protected PlayerController plrController;
    protected Rigidbody2D rb2dPlayer;
    //protected Camera cam;
    //protected CameraController cmaController;

    public GestureProfile()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        plrController = player.GetComponent<PlayerController>();
        rb2dPlayer = player.GetComponent<Rigidbody2D>();
        //cam = Camera.main;
        //cmaController = cam.GetComponent<CameraController>();
    }
    public virtual void processTapGesture(GameObject go)
    {
        plrController.processTapGesture(go);
    }
    public virtual void processTapGesture(Vector3 curMPWorld)
    {
        plrController.processTapGesture(curMPWorld);
    }
    public virtual void processHoldGesture(Vector3 curMPWorld, float holdTime, bool finished)
    {
        plrController.processHoldGesture(curMPWorld, holdTime, false);
    }
    public void processDragGesture()
    {

    }
    public void processPinchGesture()
    {

    }
}
