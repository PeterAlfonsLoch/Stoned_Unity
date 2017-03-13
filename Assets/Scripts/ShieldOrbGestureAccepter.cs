using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOrbGestureAccepter : GestureAccepter
{
    //2017-03-13: copied from ExplosionOrbGestureAccepter

    private ShieldOrbController soc;

    // Use this for initialization
    void Start()
    {
        soc = GetComponent<ShieldOrbController>();
    }

    public override bool acceptsTapGesture() { return soc.isBeingTriggered() && !soc.generatesUponContact && soc.generatesAtAll; }
    public override bool acceptsHoldGesture() { return soc.isBeingTriggered() && !soc.chargesAutomatically && soc.generatesAtAll; }

    public override void processTapGesture()
    {
        soc.trigger();
    }
    public override void processHoldGesture(bool finished)
    {
        if (!finished)
        {
            soc.charge(Time.deltaTime);
        }
        else
        {
            soc.trigger();
        }
    }
}
