using UnityEngine;

public class MilestoneActivatorURL : MilestoneActivator
{//2017-03-14: copied from MilestoneActivatorAirport

    public string url;//the url to open when activated

    public override void activateEffect()
    {
        Application.OpenURL(url);
    }
}
