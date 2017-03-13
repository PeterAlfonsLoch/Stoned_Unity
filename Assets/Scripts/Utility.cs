using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility  {

    /// <summary>
    /// Returns the given vector rotated by the given angle
    /// 2017-02-21: copied from a post by wpennypacker: https://forum.unity3d.com/threads/vector-rotation.33215/
    /// </summary>
    /// <param name="v"></param>
    /// <param name="angle"></param>
    public static Vector3 RotateZ(Vector3 v, float angle)
    {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float tx = (cos * v.x) - (sin * v.y);
        float ty = (cos * v.y) + (sin * v.x);

        return new Vector3(tx, ty);
    }

    public static Vector3 PerpendicularRight(Vector3 v)
    {
        return RotateZ(v, -Mathf.PI/2);
    }
    public static Vector3 PerpendicularLeft(Vector3 v)
    {
        return RotateZ(v, Mathf.PI / 2);
    }

    /**
    * 2016-03-25: copied from "2D Explosion Force" Asset: https://www.assetstore.unity3d.com/en/#!/content/24077
    * 2016-03-29: moved here from PlayerController
    * 2017-03-09: moved here from ForceTeleportAbility
    */
    public static void AddExplosionForce(Rigidbody2D body, float expForce, Vector3 expPosition, float expRadius)
    {
        var dir = (body.transform.position - expPosition);
        float calc = 1 - (dir.magnitude / expRadius);
        if (calc <= 0)
        {
            calc = 0;
        }

        body.AddForce(dir.normalized * expForce * calc);
    }
    /// <summary>
    /// Returns the GestureAccepter of the GameObject at the given pos, if it exists
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static GestureAccepter findGestureAccepter(Vector3 pos)
    {
        RaycastHit2D[] rch2ds = Physics2D.RaycastAll(pos,Vector3.zero);
        foreach (RaycastHit2D rch2d in rch2ds)
        {
            GestureAccepter ga = rch2d.collider.gameObject.GetComponent<GestureAccepter>();
            if (ga != null)
            {
                return ga;
            }
        }
        return null;
    }
}
