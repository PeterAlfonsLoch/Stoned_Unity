using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredWallController : SavableMonoBehaviour {
    //2017-01-24: several things copied from FloatCubeController

    public GameObject powerObj;
    public float efficiency = 100;//how much force one unit of energy can generate
    public float maxEnergyPerSecond;//the maximum amount of energy it can use per second
    public float currentEnergy;

    private Rigidbody2D rb;
    private Vector3 upDirection;//used to determine the up direction of the powered door

    // Use this for initialization
    void Start ()
    {
        upDirection = transform.up;
        rb = GetComponent<Rigidbody2D>();
    }

    public override SavableObject getSavableObject()
    {
        return new SavableObject(this, "currentEnergy", currentEnergy);
    }
    public override void acceptSavableObject(SavableObject savObj)
    {
        currentEnergy = (float)savObj.data["currentEnergy"];
    }

    void FixedUpdate()
    {
        float amountGiven = powerObj.GetComponent<PowerCubeController>().giveEnergyToObject(maxEnergyPerSecond, Time.fixedDeltaTime);
        currentEnergy += amountGiven;
        if (currentEnergy > 0)
        {
            float energyToUse = Mathf.Min(currentEnergy, maxEnergyPerSecond) * Time.fixedDeltaTime;
            if (energyToUse > 0)
            {
                currentEnergy -= energyToUse;
                Vector3 forceVector = energyToUse * efficiency * 9.81f * upDirection;
                Debug.DrawLine(transform.position, transform.position + forceVector, Color.green);
                rb.AddForce(forceVector);
            }
        }
        if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }
    }
}
