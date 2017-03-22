using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionOrbGestureAccepter : GestureAccepter {

    private ExplosionOrbController eoc;

	// Use this for initialization
	void Start () {
        eoc = GetComponent<ExplosionOrbController>();
    }
    
    public override bool acceptsTapGesture() { return ! eoc.explodesUponContact && eoc.explodesAtAll && GameManager.isInTeleportRange(gameObject); }
    public override bool acceptsHoldGesture() { return ! eoc.chargesAutomatically && eoc.explodesAtAll && GameManager.isInTeleportRange(gameObject); }

    public override void processTapGesture()
    {
        eoc.trigger();
    }
    public override void processHoldGesture(bool finished)
    {
        if (!finished)
        {
            eoc.charge(Time.deltaTime);
        }
        else
        {
            eoc.trigger();
        }
    }
}
