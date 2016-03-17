using UnityEngine;
using System.Collections;

public class MilestoneActivatorRange : MilestoneActivator
{//2016-03-17: copied from MilestoneActivator
    
    void OnTriggerEnter2D(Collider2D coll)
    {
        if ( ! used && coll.gameObject.Equals(playerObject))
        {
            sparkle();
            used = true;
            playerObject.GetComponent<PlayerController>().baseRange += incrementAmount;
            Destroy(this);//makes sure it can only be used once
        }
    }
}
