﻿using UnityEngine;
using System.Collections;

public class ExplosionOrbController : SavableMonoBehaviour {
    
    public int forceAmount = 300;
    private CircleCollider2D cc2D;
    private ForceTeleportAbility fta;
        
    private float chargeTime = 0.0f;//amount of time spent charging
    private int chargesLeft = 1;//how many charges it has left

    //Tutorial toggles
    public bool explodesUponContact = true;//false = requires mouseup (tap up) to explode
    public bool chargesAutomatically = true;//false = requires hold to charge
    public bool explodesAtAll = true;//false = doesn't do anything

    // Use this for initialization
    void Start () {
        cc2D = GetComponent<CircleCollider2D>();
        fta = GetComponent<ForceTeleportAbility>();
    }

    // Update is called once per frame
    void Update()
    {
        if (explodesAtAll && chargesLeft > 0)
        {
            
            if (explodesUponContact)
            {
                bool validTrigger = false;
                RaycastHit2D[] rch2ds = new RaycastHit2D[10];
                cc2D.Cast(Vector2.zero, rch2ds, 0, true);
                foreach (RaycastHit2D rch2d in rch2ds)
                {
                    if (rch2d && !rch2d.collider.isTrigger
                        && rch2d.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                    {
                        validTrigger = true;
                        break;
                    }
                }
                if (validTrigger)
                {
                    Debug.Log("Collision!");
                    if (chargeTime >= fta.maxHoldTime)
                    {
                        trigger();
                    }
                }
            }
            if (chargesAutomatically)
            {
                charge(Time.deltaTime);
            }
        }
    }

    public void charge(float deltaChargeTime)
    {
        chargeTime += deltaChargeTime;
        fta.processHoldGesture(transform.position, chargeTime, false);
    }

    public void trigger()
    {
        if (chargesLeft > 0)
        {
            fta.processHoldGesture(transform.position, chargeTime, true);
            chargeTime = 0;
            setChargesLeft(chargesLeft - 1);
        }
    }
    private void setChargesLeft(int charges)
    {
        chargesLeft = charges;
        if (chargesLeft > 0)
        {
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.25f, 0.25f);
        }
    }

    public override SavableObject getSavableObject()
    {
        return new SavableObject(this, "chargesLeft", chargesLeft, "chargeTime", chargeTime);
    }
    public override void acceptSavableObject(SavableObject savObj)
    {
        chargesLeft = (int)savObj.data["chargesLeft"];
        setChargesLeft(chargesLeft);
        chargeTime = (float)savObj.data["chargeTime"];
    }

}
