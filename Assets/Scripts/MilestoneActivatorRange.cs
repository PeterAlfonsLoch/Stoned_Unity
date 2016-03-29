using UnityEngine;
using System.Collections;

public class MilestoneActivatorRange : MilestoneActivator
{//2016-03-17: copied from MilestoneActivator

    public override void activateEffect()
    {
        playerObject.GetComponent<PlayerController>().baseRange += incrementAmount;
    }
}
