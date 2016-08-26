using UnityEngine;
using System.Collections;

public class GestureManager : MonoBehaviour {

    public GameObject player;
    private PlayerController plrController;
    private Rigidbody2D rb2dPlayer;
    public Camera cam;
    private CameraController cmaController;

    //Settings
    public float dragThreshold = 10;//how far from the original mouse position the current position has to be to count as a drag
    public float holdThreshold = 0.1f;//how long the tap has to be held to count as a hold (in seconds)
    public float orthoZoomSpeed = 0.5f;

    //Original Positions
    private Vector3 origMP;//"original mouse position": the mouse position at the last mouse down (or tap down) event
    private Vector3 origCP;//"original camera position": the camera offset (relative to the player) at the last mouse down (or tap down) event
    private float origTime = 0f;//"original time": the clock time at the last mouse down (or tap down) event
    //Current Positions
    private Vector3 curMP;//"current mouse position"
    private Vector3 curMPWorld;//"current mouse position world" - the mouse coordinates in the world
    private float curTime = 0f;
    //Stats
    private int touchCount = 0;//how many touches to process, usually only 0 or 1, only 2 if zoom
    private float maxMouseMovement = 0f;//how far the mouse has moved since the last mouse down (or tap down) event
    private float holdTime = 0f;//how long the gesture has been held for
    private enum ClickState {Began, InProgress, Ended, None};
    private ClickState clickState = ClickState.None;
    //Flags
    public bool cameraDragInProgress = false;
    private bool isDrag = false;
    private bool isTapGesture = true;
    private bool isHoldGesture = false;
    public const float holdTimeScale = 0.5f;
    public const float holdTimeScaleRecip = 1 / holdTimeScale;
    //Cheats
    public const bool CHEATS_ALLOWED = true;//whether or not cheats are allowed (turned off for final version)
    private int cheatTaps = 0;//how many taps have been put in for the cheat
    private float cheatTapsTime = 0f;//the time at which the cheat taps will expire
    private int cheatTapsThreshold = 3;//how many taps it takes to activate cheats
    public bool cheatsEnabled = false;//whether or not the cheats are enabled


    // Use this for initialization
    void Start () {
        plrController = player.GetComponent<PlayerController>();
        rb2dPlayer = player.GetComponent<Rigidbody2D>();
        cmaController = cam.GetComponent<CameraController>();
        Input.simulateMouseWithTouches = false;
    }
	
	// Update is called once per frame
	void Update () {
        //
        //Input scouting
        //
        if (Input.touchCount > 2)
        {
            touchCount = 0;
        }
        else if (Input.touchCount == 2)
        {
            touchCount = 2;
        }
        else if (Input.touchCount == 1)
        {
            touchCount = 1;
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                clickState = ClickState.Began;
                origMP = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                clickState = ClickState.Ended;
            }
            else
            {
                clickState = ClickState.InProgress;
                curMP = Input.GetTouch(0).position;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            touchCount = 1;
            if (Input.GetMouseButtonDown(0))
            {
                clickState = ClickState.Began;
                origMP = Input.mousePosition;
            }
            else
            {
                clickState = ClickState.InProgress;
                curMP = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            clickState = ClickState.Ended;
        }
        else if(Input.touchCount == 0 && !Input.GetMouseButton(0))
        {
            touchCount = 0;
            clickState = ClickState.None;
        }

        //
        //Preliminary Processing
        //Stats are processed here
        //
        switch (clickState)
        {
            case ClickState.Began:
                curMP = origMP;
                maxMouseMovement = 0;
                origCP = cam.transform.position - player.transform.position;
                origTime = Time.time;
                curTime = origTime;
                break;
            case ClickState.Ended: //do the same thing you would for "in progress"
            case ClickState.InProgress:
                float mm = Vector3.Distance(curMP, origMP);
                if (mm > maxMouseMovement)
                {
                    maxMouseMovement = mm;
                }
                curTime = Time.time;
                holdTime = curTime - origTime;
                break;
            case ClickState.None: break;
            default:
                throw new System.Exception("Click State of wrong type, or type not processed! (Stat Processing) clickState: " + clickState);
        }
        curMPWorld = (Vector2)cam.ScreenToWorldPoint(curMP);//cast to Vector2 to force z to 0


        //
        //Input Processing
        //
        if (touchCount == 1)
        {
            if (clickState == ClickState.Began)
            {
                //Set all flags = true
                cameraDragInProgress = false;
                isDrag = false;
                isTapGesture = true;
                isHoldGesture = false;
                if (CHEATS_ALLOWED && curMP.x < 20 && curMP.y < 20)
                {
                    cheatTaps++;
                    cheatTapsTime = Time.time + 1;//give one more second to enter taps
                    if (cheatTaps >= cheatTapsThreshold)
                    {
                        cheatsEnabled = !cheatsEnabled;
                        cheatTaps = 0;
                        cheatTapsTime = 0;
                    }
                }
            }
            else if (clickState == ClickState.InProgress)
            {
                if (maxMouseMovement > dragThreshold && rb2dPlayer.velocity.magnitude<1)
                {
                    if (!isHoldGesture)
                    {
                        isTapGesture = false;
                        isDrag = true;
                        cameraDragInProgress = true;
                    }
                }
                if (holdTime > holdThreshold)
                {
                    if (!isDrag)
                    {
                        isTapGesture = false;
                        isHoldGesture = true;
                        Time.timeScale = holdTimeScale;
                    }
                }
                if (isDrag)
                {
                    Vector3 delta = cam.ScreenToWorldPoint(origMP) - cam.ScreenToWorldPoint(curMP);
                    Vector3 newPos = player.transform.position + origCP + delta;
                    Vector3 size = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight)) - cam.ScreenToWorldPoint(new Vector3(0, 0)) + new Vector3(0, 0, 20);
                    Bounds b = new Bounds(newPos, size);
                    if (b.Contains(player.transform.position))
                    {
                        cam.transform.position = newPos;
                    }
                }
                else if (isHoldGesture)
                {
                    plrController.processHoldGesture(curMPWorld, holdTime,false);
                }
            }
            else if (clickState == ClickState.Ended)
            {
                if (isDrag)
                {
                    cmaController.pinPoint();
                }
                else if (isHoldGesture)
                {
                    plrController.processHoldGesture(curMPWorld, holdTime, true);
                }
                else if (isTapGesture)
                {
                    plrController.processTapGesture(curMPWorld);
                }

                //Set all flags = false
                cameraDragInProgress = false;
                isDrag = false;
                isTapGesture = false;
                isHoldGesture = false;
                Time.timeScale = 1;
            }
            else
            {
                throw new System.Exception("Click State of wrong type, or type not processed! (Input Processing) clickState: " + clickState);
            }
            
        }
        else {//touchCount == 0 || touchCount >= 2
            //
            //Zoom Processing
            //
            float minZoom = 1f;
            float maxZoom = player.GetComponent<PlayerController>().baseRange * cmaController.viewMultiplier;
            //
            //Mouse Scrolling Zoom
            //
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (cam.orthographicSize < maxZoom || (CHEATS_ALLOWED && cheatsEnabled))
                {
                    cmaController.adjustZoomLevel(1f);
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (cam.orthographicSize > minZoom)
                {
                    cmaController.adjustZoomLevel(-1f);
                }
            }
            //
            //Pinch Touch Zoom
            //2015-12-31 (1:23am): copied from https://unity3d.com/learn/tutorials/modules/beginner/platform-specific/pinch-zoom
            //

            // If there are two touches on the device...
            if (touchCount == 2)
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // If the camera is orthographic...
                //if (cam.orthographic)
                //{
                    // ... change the orthographic size based on the change in distance between the touches.
                    cmaController.adjustZoomLevel(deltaMagnitudeDiff * orthoZoomSpeed);
                //}
            }
            // Make sure the orthographic size never drops below zero.
            if (cam.orthographicSize < minZoom)
            {
                cmaController.setZoomLevel(minZoom);
            }
            if (!(CHEATS_ALLOWED && cheatsEnabled))//don't limit how far out they can zoom when cheats enabled
            {
                // Make sure the orthographic size never goes above maxZoom.
                if (cam.orthographicSize > maxZoom)
                {
                    cmaController.setZoomLevel(maxZoom);
                }
            }
        }

        //
        //Application closing
        //
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (cheatTapsTime <= Time.time)
        {
            //Reset cheat taps
            cheatTaps = 0;
        }
    }
}
