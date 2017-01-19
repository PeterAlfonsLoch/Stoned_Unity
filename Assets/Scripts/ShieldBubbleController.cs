using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBubbleController : SavableMonoBehaviour
{
    public float range = 3;//how big the shield is
    private float baseWidth = 0;
    private float baseHeight = 0;

    public float energy = 100;//how much energy this shield has left
    public float maxEnergy = 100;//how much energy this shield starts off with

    // Use this for initialization
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector3 bsize = sr.bounds.size;
        baseWidth = bsize.x;
        baseHeight = bsize.y;
    }

    public override SavableObject getSavableObject()
    {
        return new ShieldBubbleControllerSavable(this);
    }

    // Update is called once per frame
    //void Update () {
    //}

    public void init(float newRange, float startEnergy)
    {
        this.range = newRange;
        float size = newRange * 2;
        if (baseWidth > 0 && baseHeight > 0)
        {
            Vector3 newV = new Vector3(size / baseWidth, size / baseHeight, 0);
            transform.localScale = newV;
        }
        maxEnergy = startEnergy;
        energy = maxEnergy;
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
                energy -= force;
                //Debug.Log("SHIELD force: " + force + ", velocity: " + rb2d.velocity.magnitude+", energy: "+energy);
                if (energy <= 0)
                {
                    dissipate();
                }
            }
        }
    }

    void dissipate()
    {
        GameManager.removeObject(this.gameObject);
        Destroy(gameObject);
    }
}

//
//Class that saves the important variables of this class
//2017-01-18: copied from CrackedGroundCheckerSavable
//
public class ShieldBubbleControllerSavable : SavableObject
{
    public float range;
    public float energy;

    public ShieldBubbleControllerSavable() { }//only called by the method that reads it from the file
    public ShieldBubbleControllerSavable(ShieldBubbleController sbc)
    {
        saveState(sbc);
    }

    public override bool isSpawnedObject()
    {
        return true;
    }
    public override GameObject spawnObject()
    {
        return GameManager.getPlayerObject().GetComponent<ShieldBubbleAbility>().spawnShieldBubble(Vector2.zero, this.range, this.energy);//position will be set later in the load process
    }

    public override void loadState(GameObject go)
    {
        ShieldBubbleController sbc = go.GetComponent<ShieldBubbleController>();
        if (sbc != null)
        {
            sbc.init(this.range, this.energy);
        }
    }
    public override void saveState(SavableMonoBehaviour smb)
    {
        ShieldBubbleController sbc = ((ShieldBubbleController)smb);
        this.range = sbc.range;
        this.energy = sbc.energy;
    }
}