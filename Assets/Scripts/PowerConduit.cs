using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerConduit : SavableMonoBehaviour
{
    //2017-06-20: copied from PowerCubeController
    //These wires transfer energy from a source (such as a Shield Bubble) and convert it to energy to power other objects (such as doors)

    public GameObject lightEffect;//the object attached to it that it uses to show it is lit up
    public float maxEnergyPerSecond = 2;//(energy units per second) the max amount of energy that it can produce/move per second
    public float currentEnergyLevel = 0;//the current amount of energy it has to spend

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
        if (currentEnergyLevel < maxEnergyPerSecond)
        {
            RaycastHit2D[] rh2ds = new RaycastHit2D[100];
            bc2d.Cast(Vector2.zero, rh2ds, 0, false);
            foreach (RaycastHit2D rch2d in rh2ds)
            {
                if (rch2d && rch2d.collider != null)
                {
                    GameObject other = rch2d.collider.gameObject;
                    PowerConduit pc = other.GetComponent<PowerConduit>();
                    if (pc != null && pc.currentEnergyLevel > currentEnergyLevel)
                    {
                        float amountGiven = pc.giveEnergyToObject(maxEnergyPerSecond, Time.fixedDeltaTime);
                        currentEnergyLevel += amountGiven;
                    }
                    else
                    {
                        PowerCubeController pcc = other.GetComponent<PowerCubeController>();
                        if (pcc != null)
                        {
                            float amountGiven = pcc.giveEnergyToObject(maxEnergyPerSecond, Time.fixedDeltaTime);
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
        float amountGiven = Mathf.Min(amountRequested * deltaTime, currentEnergyLevel);
        currentEnergyLevel -= amountGiven;
        return amountGiven;
    }
}
