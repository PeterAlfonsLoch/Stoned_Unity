using UnityEngine;
using System.Collections;

public class TeleportStarUpdater : MonoBehaviour {

    //2016-03-03 copied from TeleportStreakUpdater
    public int maxTimeShown = 50;
    public Vector3 start;
    public Vector3 end;
    public int waitTime = 0;

    private float distance;
    private Vector3 newV;
    private float shrinkRate;//how much it decrements by, determined by maxTimeShown

    private int timeShown = 0;
    private bool turnedOn = false;

    // Use this for initialization
    void Start()
    {
        //position();
        turnOn(turnedOn);
        shrinkRate = (1f - 0.1f) / maxTimeShown;
    }

    // Update is called once per frame
    void Update()
    {
        if (turnedOn)
        {
            //if (timeShown == 0)
            //{
            //    position();
            //}
            if (waitTime > 0)
            {
                GetComponent<SpriteRenderer>().enabled = false; //color = new Color(prevColor.r, prevColor.g, prevColor.b, 0);
                waitTime--;
                return;
            }
            else if (waitTime == 0)
            {
                GetComponent<SpriteRenderer>().enabled = true; // color = new Color(prevColor.r, prevColor.g, prevColor.b, 1f);
                    waitTime--;
            }
            timeShown++;
            Color prevColor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(prevColor.r, prevColor.g, prevColor.b, prevColor.a - shrinkRate);
            transform.Rotate(Vector3.forward * -10);//2016-03-03: copied from an answer by Eric5h5: http://answers.unity3d.com/questions/580001/trying-to-rotate-a-2d-sprite.html
            transform.localScale -= new Vector3(shrinkRate, shrinkRate);
            if (timeShown == maxTimeShown)
            {
                Destroy(gameObject);
            }
        }
    }

    public void position()
    {
        //Set the position
        transform.position = new Vector3(start.x, start.y, 1);

        ////Set the size
        //distance = Vector3.Distance(start, end);
        //Vector3 size = GetComponent<SpriteRenderer>().bounds.size;
        //float baseWidth = size.x;
        //float baseHeight = size.y;
        //float newSize = distance;
        //newV = new Vector3(newSize / baseWidth, 1.5f * baseHeight / baseHeight);
        //transform.localScale = newV;

        ////Set the angle
        //float angle = AngleSigned(end - start, Vector3.left, Vector3.back);
        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void turnOn(bool tO)
    {
        turnedOn = tO;
        gameObject.SetActive(tO);
    }

    ////2015-12-21: copied from http://forum.unity3d.com/threads/need-vector3-angle-to-return-a-negtive-or-relative-value.51092/#post-324018
    ////originally posted by Tinus
    //public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    //{
    //    return Mathf.Atan2(
    //        Vector3.Dot(n, Vector3.Cross(v1, v2)),
    //        Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    //}
}
