using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPrincessController : MonoBehaviour
{

    public Vector3 offsetVector;
    public float holdCooldown = 1.0f;//how long between holds (sec)

    bool hasTriggered = false;
    float startHoldTime = 0;
    float soonestNextHold = 0;

    private ForceTeleportAbility fta;

    // Use this for initialization
    void Start()
    {
        fta = GetComponent<ForceTeleportAbility>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTriggered)
        {
            if (startHoldTime == 0)
            {
                startHoldTime = Time.time;
            }
            float timeDiff = Time.time - startHoldTime;
            if (timeDiff < fta.maxHoldTime)
            {
                fta.processHoldGesture(transform.position + offsetVector, timeDiff, false);
            }
            else
            {
                if (soonestNextHold == 0)
                {
                    soonestNextHold = Time.time + holdCooldown;
                    fta.processHoldGesture(transform.position + offsetVector, timeDiff, true);
                }
                else
                {
                    fta.dropHoldGesture();
                    if (soonestNextHold < Time.time)
                    {
                        soonestNextHold = 0;
                        startHoldTime = 0;
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            hasTriggered = true;
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            hasTriggered = false;
            fta.dropHoldGesture();
        }
    }
}
