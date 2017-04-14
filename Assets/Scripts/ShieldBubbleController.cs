﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBubbleController : SavableMonoBehaviour
{
    public float range = 3;//how big the shield is
    private float baseWidth = 5;//2017-01-30: if the dimensions of the sprite asset should change, then this value also needs changed
    private float baseHeight = 5;

    public float energy = 100;//how much energy this shield has left
    public float MAX_ENERGY = 100;//The max amount of energy for any shield

    private SpriteRenderer sr;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Vector3 bsize = sr.bounds.size;
        baseWidth = bsize.x;
        baseHeight = bsize.y;
        lockRB2Ds();
    }

    public override SavableObject getSavableObject()
    {
        return new SavableObject(this,
            "range", range,
            "energy", energy
            );
    }
    public override void acceptSavableObject(SavableObject savObj)
    {
        range = (float)savObj.data["range"];
        energy = (float)savObj.data["energy"];
        init(range, energy);
    }
    public override bool isSpawnedObject()
    {
        return true;
    }
    public override string getPrefabName()
    {
        return "ShieldBubble";
    }

// Update is called once per frame
void Update()
    {
        //2017-01-24: copied from WeightSwitchActivator.FixedUpdate()
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, range);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject hc = hitColliders[i].gameObject;
            if (!hc.Equals(gameObject))
            {
                if (hc.tag == "UsesEnergy")
                {
                    float amountTaken = hc.GetComponent<PowerCubeController>().takeEnergyFromSource(energy, Time.deltaTime);
                    adjustEnergy(-amountTaken);
                }
            }
        }
        if (energy <= 0)
        {
            dissipate();
        }
    }

    public void init(float newRange, float startEnergy)
    {
        this.range = newRange;
        float size = newRange * 2;
        if (baseWidth > 0 && baseHeight > 0)
        {
            Vector3 newV = new Vector3(size / baseWidth, size / baseHeight, 0);
            transform.localScale = newV;
        }
        energy = startEnergy;
        adjustEnergy(0);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (energy > 0)
        {
            GameObject other = coll.gameObject;
            Rigidbody2D rb2d = other.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                float force = rb2d.velocity.magnitude * rb2d.mass;
                checkForce(force);
                //Debug.Log("SHIELD force: " + force + ", velocity: " + rb2d.velocity.magnitude+", energy: "+energy);
            }
        }
    }

    public void checkForce(float force)
    {
        adjustEnergy(-force);
    }
    void adjustEnergy(float amount)
    {
        energy += amount;
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        //Shield Bubble VC: change its sprite's alpha value based on its energy
        Color prevColor = sr.color;
        float t = energy / MAX_ENERGY;
        sr.color = new Color(prevColor.r, prevColor.g, prevColor.b, Mathf.SmoothStep(0.2f, 1.0f, t));
    }

    void dissipate()
    {
        GameManager.destroyObject(gameObject);
    }
    void OnDestroy()
    {
        unlockRB2Ds();
    }
    ///<summary>
    ///Locks all rb2ds in its area
    ///</summary>
    void lockRB2Ds()
    {
        RaycastHit2D[] rch2ds = new RaycastHit2D[100];
        int length = GetComponent<EdgeCollider2D>().Cast(Vector2.up, rch2ds, 0, true);
        for (int i = 0; i < length; i++)
        {
            GameObject hc = rch2ds[i].collider.gameObject;
            if (hc.GetComponent<Rigidbody2D>() != null)
            {
                lockRB2D(hc);
            }
        }
    }
    void lockRB2D(GameObject go)
    {
        Rigidbody2DLock gorb2dl = go.GetComponent<Rigidbody2DLock>();
        if (gorb2dl == null)
        {
            gorb2dl = go.AddComponent<Rigidbody2DLock>();
        }
        gorb2dl.addLock(this.gameObject);
    }
    ///<summary>
    ///Unlocks all rb2ds this object locked
    ///</summary>
    void unlockRB2Ds()
    {
        List<Rigidbody2DLock> locks = new List<Rigidbody2DLock>();
        locks.AddRange(GameObject.FindObjectsOfType<Rigidbody2DLock>());
        foreach (Rigidbody2DLock rb2dlock in locks)
        {
            if (rb2dlock.holdsLock(this.gameObject))
            {
                rb2dlock.removeLock(this.gameObject);
            }
        }
    }
}