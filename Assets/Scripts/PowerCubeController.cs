using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCubeController : SavableMonoBehaviour {
    //These cubes take energy from a source (such as a Shield Bubble) and convert it to energy to power other objects (such as doors)

    public GameObject lightEffect;//the object attached to it that it uses to show it is lit up
    public float maxEnergyConsumptionPerSecond = 2;//(energy units per second) the max amount of energy that it can produce/move per second
    public float currentEnergyLevel = 0;//the current amount of energy it has to spend
    
    private SpriteRenderer lightEffectRenderer;
    private Color lightEffectColor;

    // Use this for initialization
    void Start()
    {
        lightEffectRenderer = lightEffect.GetComponent<SpriteRenderer>();
        lightEffectColor = lightEffectRenderer.color;
    }

    public override SavableObject getSavableObject()
    {
        return new SavableObject(this, "currentEnergyLevel", currentEnergyLevel);
    }
    public override void acceptSavableObject(SavableObject savObj)
    {
        currentEnergyLevel = (float)savObj.data["currentEnergyLevel"];
    }

    // Update is called once per frame
    void Update()
    {
        //2017-01-24: copied from my project: https://github.com/shieldgenerator7/GGJ-2017-Wave/blob/master/Assets/Script/CatTongueController.cs
        float newHigh = 2.0f;//opaque
        float newLow = 0.0f;//transparent
        float curHigh = maxEnergyConsumptionPerSecond;
        float curLow = 0;
        float newAlpha = ((currentEnergyLevel - curLow) * (newHigh - newLow) / (curHigh - curLow)) + newLow;
        //float newAlpha = ((currentEnergyLevel) * (newHigh) / (curHigh));
        lightEffectRenderer.color = new Color(lightEffectColor.r, lightEffectColor.g, lightEffectColor.b, newAlpha);
    }
    void LateUpdate()
    {
        if (currentEnergyLevel > maxEnergyConsumptionPerSecond)
        {
            currentEnergyLevel = maxEnergyConsumptionPerSecond;//to keep it from overloading, put in LastUpdate() to give objects a chance to use energy
        }
    }

    /// <summary>
    /// Given the max amount of energy available,
    /// it returns the amount of energy it takes
    /// </summary>
    /// <param name="maxAvailable"></param>
    /// <param name="deltaTime">so it can convert from per second to per frame</param>
    /// <returns></returns>
    public float takeEnergyFromSource(float maxAvailable, float deltaTime)
    {

        float amountTaken = Mathf.Min(maxEnergyConsumptionPerSecond-currentEnergyLevel, maxAvailable);
        currentEnergyLevel += amountTaken;
        return amountTaken;
    }
    /// <summary>
    /// Given the amount of energy requested (per second),
    /// it returns how much it can give
    /// </summary>
    /// <param name="amountRequested"></param>
    /// <param name="deltaTime">so it can convert from per second to per frame</param>
    /// <returns></returns>
    public float giveEnergyToObject(float amountRequested, float deltaTime)
    {
        float amountGiven = Mathf.Min(amountRequested*deltaTime, currentEnergyLevel);
        currentEnergyLevel -= amountGiven;
        return amountGiven;
    }
}
