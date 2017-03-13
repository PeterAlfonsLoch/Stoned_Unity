using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    private Vector3 offset;
    private Quaternion rotation;//the rotation the camera should be rotated towards
    private float scale = 1;//scale used to determine orthographicSize, independent of (landscape or portrait) orientation
    private Camera cam;
    private Rigidbody2D playerRB2D;
    private float moveTime = 0f;//used to delay the camera refocusing on the player
    private bool wasDelayed = false;
    private GestureManager gm;
    private GameManager gameManager;
    private PlayerController plyrController;

    private int prevScreenWidth;
    private int prevScreenHeight;

    struct ScalePoint
    {
        private float scalePoint;
        private bool relative;//true if relative to player's range, false if absolute
        private PlayerController plyrController;
        public ScalePoint(float scale, bool relative, PlayerController plyrController)
        {
            scalePoint = scale;
            this.relative = relative;
            this.plyrController = plyrController;
        }
        public float absoluteScalePoint()
        {
            if (relative)
            {
                return scalePoint * plyrController.baseRange;
            }
            return scalePoint;
        }
    }
    ArrayList scalePoints = new ArrayList();
    int scalePointIndex = 1;//the index of the current scalePoint in scalePoints

    // Use this for initialization
    void Start()
    {
        pinPoint();
        cam = GetComponent<Camera>();
        playerRB2D = player.GetComponent<Rigidbody2D>();
        gm = GameObject.FindGameObjectWithTag("GestureManager").GetComponent<GestureManager>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        plyrController = player.GetComponent<PlayerController>();
        scale = cam.orthographicSize;
        //Initialize ScalePoints
        scalePoints.Add(new ScalePoint(1, false, plyrController));
        scalePoints.Add(new ScalePoint(1, true, plyrController));
        scalePoints.Add(new ScalePoint(2, true, plyrController));
        scalePoints.Add(new ScalePoint(4, true, plyrController));
        //
        setScalePoint(1);

    }

    void Update()
    {
        if (prevScreenHeight != Screen.height || prevScreenWidth != Screen.width)
        {
            prevScreenWidth = Screen.width;
            prevScreenHeight = Screen.height;
            updateOrthographicSize();
        }
    }

    // Update is called once per frame, after all other objects have moved that frame
    void LateUpdate()
    {
        //transform.position = player.transform.position + offset;
        if (moveTime <= Time.time && !gm.cameraDragInProgress)
        {
            if (wasDelayed)
            {
                wasDelayed = false;
                recenter();
            }
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.transform.position + offset,
                (Vector3.Distance(
                    transform.position,
                    player.transform.position) * 1.5f + playerRB2D.velocity.magnitude)
                    * Time.deltaTime);
        }
        if (transform.rotation != rotation)
        {
            float deltaTime = 3 * Time.deltaTime;
            float angle = Quaternion.Angle(transform.rotation, rotation) * deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, deltaTime);
            offset = Quaternion.AngleAxis(angle, Vector3.forward) * offset;
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
        wasDelayed = true;
    }

    public void discardMovementDelay()
    {
        moveTime = Time.time;
        wasDelayed = false;
    }
    /// <summary>
    /// If the player position is near the tap position, discard movement delay
    /// </summary>
    /// <param name="tapPos">The world-sapce coordinate where the player tapped</param>
    /// <param name="playerPos">The world-space coordiante where the player is after teleporting</param>
    public void checkForAutoMovement(Vector3 tapPos, Vector3 playerPos)
    {
        //this times half the screen size is the maximum distance 
        //between tapPos and playerPos that will discard movementdelay early
        float DISCARD_DELAY_SENSITIVITY = 0.25f;
        //Get the average of screen width and height in world distance
        float distance = Mathf.Abs(cam.ScreenToWorldPoint(new Vector2(0, (prevScreenWidth + prevScreenHeight) / 2)).y - cam.ScreenToWorldPoint(new Vector2(0, 0)).y);
        float threshold = DISCARD_DELAY_SENSITIVITY * distance;
        if (Vector3.Distance(tapPos, playerPos) <= threshold)
        {
            discardMovementDelay();
        }
    }

    //Sets the camera's offset so it stays at this position relative to the player
    public void pinPoint()
    {
        offset = transform.position - player.transform.position;
    }

    //Recenters on Merky
    public void recenter()
    {
        offset = new Vector3(0, 0, offset.z);
    }
    //Moves the camera directly to Merky's position + offset
    public void refocus()
    {
        transform.position = player.transform.position + offset;
    }

    public void setRotation(Quaternion rotation)
    {
        this.rotation = rotation;
    }

    public void setScalePoint(int scalePointIndex)
    {
        if (scalePointIndex < 0)
        {
            scalePointIndex = 0;
        }
        else if (scalePointIndex > scalePoints.Count - 1)
        {
            scalePointIndex = scalePoints.Count - 1;
        }
        this.scalePointIndex = scalePointIndex;
        //Set the size
        scale = ((ScalePoint)scalePoints[scalePointIndex]).absoluteScalePoint();
        updateOrthographicSize();

        //Make sure player is still in view
        float width = Vector3.Distance(cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0)), cam.ScreenToWorldPoint(new Vector3(0, 0)));
        float height = Vector3.Distance(cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight)), cam.ScreenToWorldPoint(new Vector3(0, 0)));
        float radius = Mathf.Min(width, height) / 2;
        float curDistance = Vector3.Distance(player.transform.position, transform.position);
        if (curDistance > radius)
        {
            float prevZ = offset.z;
            offset = new Vector2(offset.x, offset.y).normalized * radius;
            offset.z = prevZ;
            refocus();
        }

        //GestureProfile switcher
        if (this.scalePointIndex == scalePoints.Count - 1)
        {
            gm.currentGP = gm.gestureProfiles["Rewind"];
            gameManager.showPlayerGhosts();
        }
        else
        {
            gm.currentGP = gm.gestureProfiles["Main"];
            gameManager.hidePlayerGhosts();
        }
    }
    public void adjustScalePoint(int addend)
    {
        setScalePoint(scalePointIndex + addend);
    }
    public int getScalePointIndex()
    {
        return scalePointIndex;
    }
    public void updateOrthographicSize()
    {
        if (Screen.height > Screen.width)//portrait orientation
        {
            cam.orthographicSize = (scale * cam.pixelHeight) / cam.pixelWidth;
        }
        else {//landscape orientation
            cam.orthographicSize = scale;
        }
    }
}
