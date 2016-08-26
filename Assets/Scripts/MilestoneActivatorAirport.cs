using UnityEngine;

public class MilestoneActivatorAirport : MilestoneActivator
{//2016-03-29: copied from MilestoneActivatorForceTeleport

    public override void activateEffect()
    {
        playerObject.GetComponent<PlayerController>().maxAirPorts += incrementAmount;
    }
}
