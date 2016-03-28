using UnityEngine;
using System.Collections;

public class GestureManager : MonoBehaviour {

    public GameObject player;
    private PlayerController plrController;
    public Camera cameraMain;
    private CameraController cmaController; 

    private bool isTeleportGesture;
    private float maxMouseMovement = 0f;//how far the mouse has moved since the last mouse down (or tap down) event
    private Vector3 origMP;//"original mouse position": the mouse position at the last mouse down (or tap down) event

    // Use this for initialization
    void Start () {
        plrController = player.GetComponent<PlayerController>();
        cmaController = cameraMain.GetComponent<CameraController>();
	}
	
	// Update is called once per frame
	void Update () {
        float dragThreshold = 10;
        if (Input.touchCount == 0)
        {
            isTeleportGesture = true;
        }
        else if (Input.touchCount >= 2)
        {
            isTeleportGesture = false;
        }
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                maxMouseMovement = 0;
                origMP = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (dragThreshold == 0 && maxMouseMovement < 50)
                {
                    dragThreshold = maxMouseMovement;
                }
                if (isTeleportGesture)//don't let the pinch zoom gesture count as a teleport gesture
                {
                    plrController.teleport(false);
                }
            }
            float mm = Vector3.Distance(Input.GetTouch(0).position, origMP);
            if (mm > maxMouseMovement)
            {
                maxMouseMovement = mm;
            }
            if (maxMouseMovement > dragThreshold)
            {
                isTeleportGesture = false;
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                float mm = Vector3.Distance(Input.mousePosition, origMP);
                if (mm > maxMouseMovement)
                {
                    maxMouseMovement = mm;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                maxMouseMovement = 0;
                origMP = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (dragThreshold == 0 && maxMouseMovement < 50)
                {
                    dragThreshold = maxMouseMovement;
                }
                if (isTeleportGesture && maxMouseMovement <= dragThreshold)
                {
                    plrController.teleport(true);
                }
            }
        }
    }
}
