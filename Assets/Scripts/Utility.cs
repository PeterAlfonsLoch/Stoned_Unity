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
}
