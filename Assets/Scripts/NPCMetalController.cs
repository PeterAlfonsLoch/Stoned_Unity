using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMetalController : NPCController {

    public GameObject powerCube;//the cube to check to see if it's powered or not
    PowerCubeController pcc;
    public float powerConsumptionRate = 3.0f;

    private float lastPowerReturned;//the last amount of power it got

    protected override void Start()
    {
        base.Start();
        pcc = powerCube.GetComponent<PowerCubeController>();
    }

    protected override bool greetOnlyOnce()
    {
        return false;
    }

    protected override bool canGreet()
    {
        lastPowerReturned = pcc.giveEnergyToObject(powerConsumptionRate, Time.deltaTime);
        return lastPowerReturned > 0;
    }

    protected override bool shouldStop()
    {
        return lastPowerReturned <= 0;
    }
}
