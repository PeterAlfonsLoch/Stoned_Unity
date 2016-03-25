using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;
    private Camera cam;
    private Rigidbody2D playerRB2D;
    private float moveTime = 0f;//used to delay the camera refocusing on the player
    private float maxMouseMovement = 0f;//how far the mouse has moved since the last mouse down (or tap down) event
    private Vector3 origMP;//"original mouse position": the mouse position at the last mouse down (or tap down) event
    private Vector3 origCP;//"original camera position": the camera position (in world coordinates) at the last mouse down (or tap down) event
    private bool cameraDragInProgress = false;

    public float perspectiveZoomSpeed = 0.5f; 
    public float orthoZoomSpeed = 0.5f;

    // Use this for initialization
    void Start () {
		offset = transform.position - player.transform.position;
        cam = GetComponent<Camera>();
        playerRB2D = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float minZoom = 1f;
        float maxZoom = player.GetComponent<PlayerController>().baseRange * 2;
        //
        //Mouse Scrolling Zoom
        //
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (cam.orthographicSize < maxZoom)
            {
                cam.orthographicSize += 1f;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (cam.orthographicSize > minZoom)
            {
                cam.orthographicSize -= 1f;
            }
        }

        //
        //Pinch Touch Zoom
        //2015-12-31 (1:23am): copied from https://unity3d.com/learn/tutorials/modules/beginner/platform-specific/pinch-zoom
        //

        // If there are two touches on the device...
        if (Input.touchCount == 2)
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
            if (cam.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
            }
            else
            {
                // Otherwise change the field of view based on the change in distance between the touches.
                cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 0 and 180.
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 0.1f, 179.9f);
            }
        }

        // Make sure the orthographic size never drops below zero.
        cam.orthographicSize = Mathf.Max(cam.orthographicSize, minZoom);
        // Make sure the orthographic size never goes above maxZoom.
        cam.orthographicSize = Mathf.Min(cam.orthographicSize, maxZoom);

        //
        //Manual Camera Movement
        //
        float dragThreshold = 10;
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                maxMouseMovement = 0;
                origCP = transform.position - player.transform.position;
                origMP = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (cameraDragInProgress)
                {
                    cameraDragInProgress = false;
                    offset = transform.position - player.transform.position;
                }
            }
            else
            {
                if (maxMouseMovement > dragThreshold)
                {
                    cameraDragInProgress = true;
                    Vector3 delta = Camera.main.ScreenToWorldPoint(origMP) - Camera.main.ScreenToWorldPoint((Vector3)Input.GetTouch(0).position);
                    Vector3 newPos = player.transform.position + origCP + delta;
                    Vector3 size = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight)) - Camera.main.ScreenToWorldPoint(new Vector3(0, 0)) + new Vector3(0, 0, 20);
                    Bounds b = new Bounds(newPos, size);
                    if (b.Contains(player.transform.position))
                    {
                        transform.position = newPos;
                    }
                }
            }
            float mm = Vector3.Distance(Input.GetTouch(0).position, origMP);
            if (mm > maxMouseMovement)
            {
                maxMouseMovement = mm;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            float mm = Vector3.Distance(Input.mousePosition, origMP);
            if (mm > maxMouseMovement)
            {
                maxMouseMovement = mm;
            }
            if (Input.GetMouseButtonDown(0))
            {
                maxMouseMovement = 0;
                origCP = transform.position - player.transform.position;
                origMP = Input.mousePosition;
            }

            else
            {
                if (maxMouseMovement > dragThreshold)
                {
                    cameraDragInProgress = true;
                    Vector3 delta = Camera.main.ScreenToWorldPoint(origMP) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 newPos = player.transform.position + origCP + delta;
                    Vector3 size = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight)) - Camera.main.ScreenToWorldPoint(new Vector3(0, 0)) + new Vector3(0, 0, 20);
                    Bounds b = new Bounds(newPos, size);
                    if (b.Contains(player.transform.position))
                    {
                        transform.position = newPos;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (cameraDragInProgress)
            {
                cameraDragInProgress = false;
                offset = transform.position - player.transform.position;
            }
        }

        //
        //Application closing
        //
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Update is called once per frame, after all other objects have moved that frame
    void LateUpdate()
    {
        //transform.position = player.transform.position + offset;
        if (moveTime <= Time.time && !cameraDragInProgress)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.transform.position + offset,
                (Vector3.Distance(
                    transform.position,
                    player.transform.position) * 2 + playerRB2D.velocity.magnitude)
                    * Time.deltaTime);
        }
    }

    /**
    * Makes sure that the current camera movement delay is at least the given delay amount in seconds
    * @param delayAmount How much to delay camera movement by in seconds
    */
    public void delayMovement(float delayAmount)
    {        
        if (moveTime < Time.time + delayAmount)
        {
            moveTime = Time.time + delayAmount;
        }
    }

    public void discardMovementDelay()
    {
        moveTime = Time.time;
    }
}
