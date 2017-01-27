using UnityEngine;
using System.Collections;

public class ForceTeleportAbility : PlayerAbility
{
    public GameObject forceRangeIndicator;//prefab
    private TeleportRangeIndicatorUpdater friu;//"force range indicator updater"
    private GameObject frii;//"force range indicator instance"
    public GameObject explosionEffect;

    public float maxForceAmount = 5000;
    public float forceAmount = 10;//how much force to apply = forceAmount * 2^(holdTime*10)
    public float maxRange = 3;
    public float maxHoldTime = 1;//how long until the max range is reached

    public AudioClip forceTeleportSound;

    public new bool takesGesture()
    {
        return true;
    }

    public new bool takesHoldGesture()
    {
        return true;
    }

    public new void processHoldGesture(Vector3 pos, float holdTime, bool finished)
    {
        float range = maxRange * holdTime*GestureManager.holdTimeScaleRecip / maxHoldTime;
        if (range > maxRange)
        {
            range = maxRange;
        }
        if (finished)
        {
            pos = (Vector2)pos;//2016-03-29: for some reason, pos.z is -10 when it comes in
            float force = forceAmount * Mathf.Pow(2, (holdTime * 10 * GestureManager.holdTimeScaleRecip));
            if (force > maxForceAmount)
            {
                force = maxForceAmount;
            }
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pos, range);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                Rigidbody2D orb2d = hitColliders[i].gameObject.GetComponent<Rigidbody2D>();
                if (orb2d != null)
                {
                    AddExplosionForce(orb2d, force, pos, range);
                }
                else
                {
                    CrackedGroundChecker cgc = hitColliders[i].gameObject.GetComponent<CrackedGroundChecker>();
                    if (cgc != null)
                    {
                        cgc.checkForce(force);
                    }
                    ShieldBubbleController sbc = hitColliders[i].gameObject.GetComponent<ShieldBubbleController>();
                    if (sbc != null)
                    {
                        sbc.checkForce(force);
                    }
                }
            }
            showExplosionEffect(transform.position, pos, range*2);
            AudioSource.PlayClipAtPoint(forceTeleportSound, pos);
            Destroy(frii);
            frii = null;
        }
        else {
            if (frii == null)
            {
                frii = Instantiate(forceRangeIndicator);
                friu = frii.GetComponent<TeleportRangeIndicatorUpdater>();
                frii.GetComponent<SpriteRenderer>().enabled = false;
            }
            frii.transform.position = (Vector2)pos;
            friu.setSize(range * 2);
        }
    }

    public void dropHoldGesture()
    {
        if (frii != null)
        {
            Destroy(frii);
            frii = null;
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
    
    void showExplosionEffect(Vector2 oldp, Vector2 newp, float finalSize)
    {
        GameObject newTS = (GameObject)Instantiate(explosionEffect);
        newTS.GetComponent<ExplosionEffectUpdater>().start = oldp;
        newTS.GetComponent<ExplosionEffectUpdater>().end = newp;
        newTS.GetComponent<ExplosionEffectUpdater>().finalSize = finalSize;
        newTS.GetComponent<ExplosionEffectUpdater>().position();
        newTS.GetComponent<ExplosionEffectUpdater>().init();
        newTS.GetComponent<ExplosionEffectUpdater>().turnOn(true);
    }
}
