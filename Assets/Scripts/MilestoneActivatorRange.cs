using UnityEngine;
using System.Collections;

public class MilestoneActivatorRange : MilestoneActivator
{//2016-03-17: copied from MilestoneActivator

    public override void activateEffect()
    {
        GameManager.getPlayerObject().GetComponent<PlayerController>().baseRange += incrementAmount;
    }
}
