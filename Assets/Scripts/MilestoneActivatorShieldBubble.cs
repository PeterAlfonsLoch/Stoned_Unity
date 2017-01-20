using UnityEngine;

public class MilestoneActivatorShieldBubble : MilestoneActivator
{//2017-01-19: copied from MilestoneActivatorForceTeleport

    public override void activateEffect()
    {
        GameManager.getPlayerObject().GetComponent<ShieldBubbleAbility>().enabled = true;
    }
}
