using UnityEngine;
using System.Collections;

public class WallClimbAbility : PlayerAbility
{//2017-03-17: copied from ForceTeleportAbility

    public AudioClip wallClimbSound;

    public new bool takesGesture()
    {
        return false;
    }

    public new bool takesHoldGesture()
    {
        return false;
    }

    /// <summary>
    /// Not used. See PlayerController.isGrounded()
    /// </summary>
    /// <returns></returns>
    //public bool isGrounded(){}

    public void playWallClimbEffects(Vector2 pos)
    {
        particleSystem.transform.position = pos;
        particleSystem.Play();
    }
}
