﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerConduit : SavableMonoBehaviour
{
    //2017-06-20: copied from PowerCubeController
    //These wires transfer energy from a source (such as a Shield Bubble) and convert it to energy to power other objects (such as doors)

    public GameObject lightEffect;//the object attached to it that it uses to show it is lit up
    public float maxEnergyPerSecond = 2;//(energy units per second) the max amount of energy that it can produce/move per second
    public float currentEnergyLevel = 0;//the current amount of energy it has to spend
    public bool givesEnergy = true;//whether this can give power to other PowerConduits
    public bool takesEnergy = true;//whether this can take power from other PowerConduits
    public bool convertsToEnergy = false;//whether this can convert other sources to energy
    public bool usesEnergy = false;//whether this uses energy to power some mechanism

    private SpriteRenderer lightEffectRenderer;
    private Color lightEffectColor;
    private BoxCollider2D bc2d;

    // Use this for initialization
    void Start()
    {
        lightEffectRenderer = lightEffect.GetComponent<SpriteRenderer>();
        lightEffectColor = lightEffectRenderer.color;
        bc2d = GetComponent<BoxCollider2D>();
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
        float curHigh = maxEnergyPerSecond;
        float curLow = 0;
        float newAlpha = ((currentEnergyLevel - curLow) * (newHigh - newLow) / (curHigh - curLow)) + newLow;
        //float newAlpha = ((currentEnergyLevel) * (newHigh) / (curHigh));
        lightEffectRenderer.color = new Color(lightEffectColor.r, lightEffectColor.g, lightEffectColor.b, newAlpha);
    }
    void FixedUpdate()
    {
        if (takesEnergy && currentEnergyLevel < maxEnergyPerSecond)
        {
            RaycastHit2D[] rh2ds = new RaycastHit2D[100];
            bc2d.Cast(Vector2.zero, rh2ds, 0, false);
            foreach (RaycastHit2D rch2d in rh2ds)
            {
                if (rch2d && rch2d.collider != null)
                {
                    GameObject other = rch2d.collider.gameObject;
                    PowerConduit pc = other.GetComponent<PowerConduit>();
                    if (pc != null)
                    {
                        if (pc.givesEnergy && (!pc.takesEnergy || pc.currentEnergyLevel > currentEnergyLevel))
                        {
                            float amountGiven = pc.giveEnergyToObject(maxEnergyPerSecond - currentEnergyLevel, 1);
                            currentEnergyLevel += amountGiven;
                        }
                    }
                }
            }
        }
    }
    void LateUpdate()
    {
        if (currentEnergyLevel > maxEnergyPerSecond)
        {
            currentEnergyLevel = maxEnergyPerSecond;//to keep it from overloading, put in LastUpdate() to give objects a chance to use energy
        }
        if (currentEnergyLevel < 0)
        {
            currentEnergyLevel = 0;
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
        if (!takesEnergy)
        {
            throw new System.MethodAccessException("PowerConduit.takeEnergyFromSource(..) should not be called on this object because its takesEnergy var is: " + takesEnergy);
        }
        float amountTaken = Mathf.Min(maxEnergyPerSecond - currentEnergyLevel, maxAvailable);
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
        if (!givesEnergy)
        {
            throw new System.MethodAccessException("PowerConduit.giveEnergyToObject(..) should not be called on this object because its givesEnergy var is: " + givesEnergy);
        }
        float amountGiven = Mathf.Min(amountRequested * deltaTime, currentEnergyLevel);
        currentEnergyLevel -= amountGiven;
        return amountGiven;
    }
    /// <summary>
    /// Given the max amount of energy available to convert,
    /// it returns the amount of energy it converts
    /// Used to create energy for a power system
    /// </summary>
    /// <param name="maxAvailable"></param>
    /// <param name="deltaTime">so it can convert from per second to per frame</param>
    /// <returns></returns>
    public float convertSourceToEnergy(float maxAvailable, float deltaTime)
    {
        if (!convertsToEnergy)
        {
            throw new System.MethodAccessException("PowerConduit.convertSourceToEnergy(..) should not be called on this object because its convertsToEnergy var is: " + convertsToEnergy);
        }
        float amountTaken = Mathf.Min(maxEnergyPerSecond - currentEnergyLevel, maxAvailable);
        currentEnergyLevel += amountTaken;
        return amountTaken;
    }
    /// <summary>
    /// Given the amount of energy requested (per second),
    /// it returns how much it can give to be used
    /// Used to power a mechanism, reducing the energy in a power system
    /// </summary>
    /// <param name="amountRequested"></param>
    /// <param name="deltaTime">so it can convert from per second to per frame</param>
    /// <returns></returns>
    public float useEnergy(float amountRequested, float deltaTime)
    {
        if (!usesEnergy) {
            throw new System.MethodAccessException("PowerConduit.useEnergy(..) should not be called on this object because its usesEnergy var is: " + usesEnergy);
        }
        float amountGiven = Mathf.Min(amountRequested * deltaTime, currentEnergyLevel);
        currentEnergyLevel -= amountGiven;
        return amountGiven;
    }
}
