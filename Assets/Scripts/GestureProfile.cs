using UnityEngine;
using System.Collections;

public class GestureProfile {

    protected GameObject player;
    protected PlayerController plrController;
    protected Rigidbody2D rb2dPlayer;
    //protected Camera cam;
    //protected CameraController cmaController;
    protected GameManager gm;

    public GestureProfile()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        plrController = player.GetComponent<PlayerController>();
        rb2dPlayer = player.GetComponent<Rigidbody2D>();
        //cam = Camera.main;
        //cmaController = cam.GetComponent<CameraController>();
        gm = GameObject.FindObjectOfType<GameManager>();
    }
    public virtual void processTapGesture(GameObject go)
    {
        plrController.processTapGesture(go);
        gm.Save();
    }
    public virtual void processTapGesture(Vector3 curMPWorld)
    {
        plrController.processTapGesture(curMPWorld);
        gm.Save();
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
