using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAccepter : MonoBehaviour {
    //used for objects that need to know their gravity direction

    private Vector2 gravityVector;
    public Vector2 Gravity
    {
        get { return gravityVector; }
        set
        {
            // 2017-06-02: moved here from PlayerController.setGravityVector(.)
            if (value.x != gravityVector.x || value.y != gravityVector.y)
            {
                gravityVector = value;
                //v = P2 - P1    //2016-01-10: copied from an answer by cjdev: http://answers.unity3d.com/questions/564166/how-to-find-perpendicular-line-in-2d.html
                //P3 = (-v.y, v.x) / Sqrt(v.x ^ 2 + v.y ^ 2) * h
                sideVector = new Vector3(-gravityVector.y, gravityVector.x) / Mathf.Sqrt(gravityVector.x * gravityVector.x + gravityVector.y * gravityVector.y);
            }
        }
    }
    private Vector2 sideVector;
    public Vector2 SideVector
    {
        get { return sideVector; }
        private set { sideVector = value; }
    }
}
