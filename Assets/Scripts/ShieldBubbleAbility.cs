using UnityEngine;
using System.Collections;

public class ShieldBubbleAbility : PlayerAbility
{//2017-01-17: copied from ForceTeleportAbility
    public GameObject shieldRangeIndicator;//prefab
    private TeleportRangeIndicatorUpdater sriu;//"shield range indicator updater"
    private GameObject srii;//"shield range indicator instance"
    public GameObject shieldBubblePrefab;//prefab used to spawn shield bubbles
    public float maxRange = 2.5f;
    public float maxHoldTime = 1;//how long until the max range is reached

    public AudioClip shieldBubbleSound;
    public AudioClip shieldBubbleFailSound;//played when a shield cannot be made because it overlaps another shield

    public new bool takesGesture()
    {
        return true;
    }

    public new bool takesHoldGesture()
    {
        return true;
    }

    public void processHoldGesture(Vector2 pos, float holdTime, bool finished)
    {
        float range = maxRange * holdTime * GestureManager.holdTimeScaleRecip / maxHoldTime;
        if (range > maxRange)
        {
            range = maxRange;
        }
        if (finished)
        {
            //Spawn Shield Bubble
            if (canSpawnShieldBubble(pos, range))
            {
                GameObject newSB = spawnShieldBubble(pos, range, 100 * range / maxRange);
                GameManager.addObject(newSB);
                AudioSource.PlayClipAtPoint(shieldBubbleSound, pos);
            }
            else
            {
                AudioSource.PlayClipAtPoint(shieldBubbleFailSound, pos);
            }
            Destroy(srii);
            srii = null;
        }
        else {
            if (srii == null)
            {
                srii = Instantiate(shieldRangeIndicator);
                sriu = srii.GetComponent<TeleportRangeIndicatorUpdater>();
                srii.GetComponent<SpriteRenderer>().enabled = false;
            }
            srii.transform.position = (Vector2)pos;
            sriu.setRange(range);
        }
    }

    public void dropHoldGesture()
    {
        if (srii != null)
        {
            Destroy(srii);
            srii = null;
        }
    }

    public bool canSpawnShieldBubble(Vector2 pos, float range)
    {
        //Check to make sure it won't overlap another shield bubble
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pos, range);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.GetComponent<ShieldBubbleController>() != null)
            {
                //found another shield bubble
                return false;
            }
        }
        return true;
    }

    public GameObject spawnShieldBubble(Vector2 pos, float range, float energy)
    {
        //Spawn New Shield Bubble
        GameObject newSB = (GameObject)Instantiate(shieldBubblePrefab);
        newSB.transform.position = pos;
        newSB.GetComponent<ShieldBubbleController>().init(range, energy);
        newSB.name += System.DateTime.Now.Ticks;
        return newSB;
    }
}
