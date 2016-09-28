using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    public int viewMultiplier = 2;
    public int mapViewMultiplier = 1;

    private Vector3 offset;
    private float scale = 1;//scale used to determine orthographicSize, independent of (landscape or portrait) orientation
    private Camera cam;
    private Rigidbody2D playerRB2D;
    private float moveTime = 0f;//used to delay the camera refocusing on the player
    private bool wasDelayed = false;
    private GestureManager gm;
    private PlayerController plyrController;

    private int prevScreenWidth;
    private int prevScreenHeight;
    float minZoom = 1f;

    const int LOWER_BOUND = -1;
    const int NONBOUND = 0;
    const int UPPER_BOUND = 1;
    struct ScalePoint{
        public float scalePoint;
        float radius;
        public int bound;
        public bool isExtreme;//{ get { return isExtreme; } set { isExtreme = value; } }//is lowest or highest
        public ScalePoint(float scale, float radius, int bound, bool extreme)
        {
            scalePoint = scale;
            this.radius = radius;
            this.bound = bound;
            isExtreme = extreme;
        }
        public bool breaksBounds(float scale)
        {
            if ((bound == LOWER_BOUND && scale < scalePoint)
                || (bound == UPPER_BOUND && scale > scalePoint))
            {
                return true;
            }
            return false;
        }
        public float bounds(float scale)
        {
            if ((bound == LOWER_BOUND && scale < scalePoint)
                || (bound == UPPER_BOUND && scale > scalePoint))
            {
                return scalePoint;
            }
            return scale;
        }
        public float attracts(float scale)
        {
            if (Mathf.Abs(scalePoint - scale) <= radius)
            {
                return scalePoint;
            }
                return scale;
        }
    }
    ArrayList scalePoints = new ArrayList();

    // Use this for initialization
    void Start()
    {
        pinPoint();
        cam = GetComponent<Camera>();
        playerRB2D = player.GetComponent<Rigidbody2D>();
        gm = GameObject.FindGameObjectWithTag("GestureManager").GetComponent<GestureManager>();
        plyrController = player.GetComponent<PlayerController>();
        scale = cam.orthographicSize;
        //Initialize ScalePoints
        scalePoints.Add(new ScalePoint(1, 1, LOWER_BOUND, true));
        scalePoints.Add(new ScalePoint(7, 2, UPPER_BOUND, false));
        scalePoints.Add(new ScalePoint(20, 10, UPPER_BOUND, true));
        //ScalePoint sp=((ScalePoint)scalePoints[1]);
        //sp.isExtreme = false;
        //TODO 2016-09-27
        //float min = float.MaxValue, max = 0;
        //int minIndex, maxIndex;

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

    public void setZoomLevel(float level)
    {
        //// Make sure the orthographic size never drops below zero.
        //if (level < minZoom)
        //{
        //    level = minZoom;
        //}
        //if (!(GestureManager.CHEATS_ALLOWED && gm.cheatsEnabled))//don't limit how far out they can zoom when cheats enabled
        //{
        //    float maxZoom = plyrController.baseRange * viewMultiplier * mapViewMultiplier;//landscape
        //    if (level > maxZoom)
        //    {
        //        level = maxZoom;
        //    }
        //}
        foreach (ScalePoint sp in scalePoints)
        {
            if (sp.isExtreme)//if extreme, don't let it go beyond
            {
                level = sp.bounds(level);
                Debug.Log("sp.ie: " + sp.isExtreme);
            }
            else
            {
                if (sp.bound != NONBOUND && sp.breaksBounds(level))//if it goes beyond your bound, try to make it snap to another bound
                {
                    foreach (ScalePoint sp2 in scalePoints)
                    {
                        if (sp2.isExtreme && sp2.bound == sp.bound)//if sp2 is the outer most bound to sp
                        {
                            if (sp2.attracts(level) == sp2.scalePoint && level != sp2.scalePoint && ! sp2.breaksBounds(level))//if the level is within range of the sp2 but not right on it
                            {
                                level = sp.scalePoint;//snap it to sp (because the player is prob trying to zoom in)
                            }
                            else {
                                level = sp2.scalePoint;//snap to sp2 (because player is prob trying to zoom out)
                            }
                            break;
                        }
                    }
                }
            }
        }

        //Set the size
        scale = level;
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
    }
    public void adjustZoomLevel(float addend)
    {
        setZoomLevel(scale + addend);
    }
    public void updateOrthographicSize()
    {
        float level = scale;
        //See if the level snaps to any scalePoint
        foreach (ScalePoint sp in scalePoints)
        {
            level = sp.attracts(level);
        }
        if (Screen.height > Screen.width)//portrait orientation
        {
            cam.orthographicSize = (level * cam.pixelHeight) / cam.pixelWidth;
        }
        else {//landscape orientation
            cam.orthographicSize = level;
        }
    }
}
