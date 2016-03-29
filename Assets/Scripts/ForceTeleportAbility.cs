using UnityEngine;
using System.Collections;

public class ForceTeleportAbility : PlayerAbility {

    public float maxForceAmount = 5000;
    public float forceAmount = 10;//how much force to apply = forceAmount * 2^(holdTime*10)
    public float maxRange = 3;

    public new bool takesGesture()
    {
        return true;
    }

    public new bool takesHoldGesture()
    {
        return true;
    }

    public new void processHoldGesture(Vector3 pos, float holdTime)
    {
        pos = (Vector2)pos;//2016-03-29: for some reason, pos.z is -10 when it comes in
        float force = forceAmount * Mathf.Pow(2,(holdTime * 10));
        if (force > maxForceAmount)
        {
            force = maxForceAmount;
        }
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pos, maxRange);
        //Debug.Log("hitColliders: " + hitColliders+" ["+hitColliders.Length+"]");
        for (int i = 0; i < hitColliders.Length; i++)
        {
            Rigidbody2D orb2d = hitColliders[i].gameObject.GetComponent<Rigidbody2D>();
            if (orb2d != null)
            {
                AddExplosionForce(orb2d, force, pos, maxRange);
            }
        }
    }

    /**
    * 2016-03-25: copied from "2D Explosion Force" Asset: https://www.assetstore.unity3d.com/en/#!/content/24077
    * 2016-03-29: moved here from PlayerController
    */
    static void AddExplosionForce(Rigidbody2D body, float expForce, Vector3 expPosition, float expRadius)
    {
        var dir = (body.transform.position - expPosition);
        float calc = 1 - (dir.magnitude / expRadius);
        if (calc <= 0)
        {
            calc = 0;
        }

        body.AddForce(dir.normalized * expForce * calc);
    }
}
