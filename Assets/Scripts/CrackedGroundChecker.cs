using UnityEngine;
using System.Collections;

public class CrackedGroundChecker : SavableMonoBehaviour
{

    public bool cracked = false;//whether or not it has been broken

    public float forceThreshold = 10;//how much force is required to break it
    public AudioClip breakSound;
    public GameObject rubble;//prefab for the rubble blocks
    public GameObject secretHider;//the hidden area to be shown when this cracked ground breaks
    public GameObject secretHider2;//the other one, if the level was traversed in reverse

    private BoxCollider2D bc2d;

    void Start()
    {
        bc2d = gameObject.GetComponent<BoxCollider2D>();
    }

    public override SavableObject getSavableObject()
    {
        return new CrackedGroundCheckerSavable(this);
    }

    void setCracked(bool nowCracked)
    {
        this.cracked = nowCracked;
        if (!cracked)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            bc2d.enabled = true;
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            bc2d.enabled = false;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!cracked)
        {
            GameObject other = coll.gameObject;
            Rigidbody2D rb2d = other.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                float force = rb2d.velocity.magnitude * rb2d.mass;
                Debug.Log("force: " + force + ", velocity: " + rb2d.velocity.magnitude);
                checkForce(force);
            }
        }
    }

    public void checkForce(float force)
    {
        if (force > forceThreshold)
        {
            crackAndBreak();
        }
    }

    private void crackAndBreak()
    {
        AudioSource.PlayClipAtPoint(breakSound, transform.position);
        if (secretHider != null)
        {
            secretHider.AddComponent<Fader>();
        }
        if (secretHider2 != null)
        {
            secretHider2.AddComponent<Fader>();
        }
        if (rubble != null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            Bounds b = sr.bounds;
            float x1 = b.min.x;
            float x2 = b.max.x;
            float y1 = b.min.y;
            float y2 = b.max.y;
            //instantiate 4 rubble blocks
            ArrayList nrList = new ArrayList();
            for (int ip = 1; ip < 8; ip += 2)//1,3,5,7
            {
                //Create rubble and copy data into it
                float xpos = (x2 - x1) * ip / 8 + x1;
                float ypos = (y2 - y1) * ip / 8 + y1;
                GameObject nr = (GameObject)Instantiate(rubble);
                nr.transform.localScale = transform.localScale;
                nr.transform.rotation = transform.rotation;
                nr.transform.position = new Vector2(xpos, ypos);
                nr.AddComponent<Fader>();//make the rubble go away eventually
                nr.GetComponent<Fader>().delayTime = 1;
                nrList.Add(nr);
            }
            foreach (GameObject nr in nrList)
            {
                //Check to see if it should be static
                SpriteRenderer nrsr = nr.GetComponent<SpriteRenderer>();
                Bounds nrb = nrsr.bounds;
                float rubbleBoundsThreshold = 0.7f;
                nrb.extents = new Vector2(nrb.extents.x * rubbleBoundsThreshold, nrb.extents.y * rubbleBoundsThreshold);
                {
                    Debug.DrawLine(new Vector2(nrb.min.x, nrb.max.y), new Vector2(nrb.max.x, nrb.max.y), Color.black, 5);
                    Debug.DrawLine(new Vector2(nrb.min.x, nrb.min.y), new Vector2(nrb.max.x, nrb.min.y), Color.black, 5);
                }
                Collider2D[] hitColliders = Physics2D.OverlapAreaAll(nrb.min, nrb.max);
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    GameObject hc = hitColliders[i].gameObject;
                    //Is the rubble intersecting a static object?
                    if (hc.Equals(gameObject) || nrList.Contains(hc))
                    {
                        continue;
                    }
                    Rigidbody2D orb2d = hc.GetComponent<Rigidbody2D>();
                    if (orb2d == null)
                    {
                        if (hc.GetComponent<BoxCollider2D>() || hc.GetComponent<PolygonCollider2D>())
                        {
                            if (!hc.transform.parent.gameObject.tag.Equals("HideableArea"))
                            {
                                Debug.Log("Intersecting static object: " + hc);
                                //If it is, make the rubble static
                                Destroy(nr.GetComponent<Rigidbody2D>());
                            }
                        }
                    }
                }
            }
        }
        setCracked(true);
    }

    //
    //Inner class that saves the important variables of this class
    //
    public class CrackedGroundCheckerSavable : SavableObject
    {
        bool cracked;

        public CrackedGroundCheckerSavable(CrackedGroundChecker cgc)
        {
            saveState(cgc);
        }

        public override void loadState(GameObject go)
        {
            CrackedGroundChecker cgc = go.GetComponent<CrackedGroundChecker>();
            if (cgc != null)
            {
                cgc.setCracked(this.cracked);
            }
        }
        public override void saveState(SavableMonoBehaviour smb)
        {
            CrackedGroundChecker cgc = ((CrackedGroundChecker)smb);
            this.cracked = cgc.cracked;
        }
    }

}