using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    private Vector3 offset;
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
    
    struct ScalePoint{
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
        wasDelayed = true;
    }

    public void discardMovementDelay()
    {
        moveTime = Time.time;
        wasDelayed = false;
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
        Vector3 size = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight)) - cam.ScreenToWorldPoint(new Vector3(0, 0)) + new Vector3(0, 0, 20);
        Bounds b = new Bounds(cam.transform.position, size);
        if (!b.Contains(player.transform.position))
        {
            float newX = offset.x;
            float newY = offset.y;
            Vector3 ppos = player.transform.position;
            if (ppos.x < b.min.x)
            {
                newX = b.extents.x;
            }
            else if (ppos.x > b.max.x)
            {
                newX = -b.extents.x;
            }
            if (ppos.y < b.min.y)
            {
                newY = b.extents.y;
            }
            else if (ppos.y > b.max.y)
            {
                newY = -b.extents.y;
            }
            offset = new Vector3(newX, newY, offset.z);
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
