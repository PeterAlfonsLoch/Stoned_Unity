using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardMaterial : SavableMonoBehaviour {

    public float hardness = 1.0f;
    public float maxIntegrity = 100f;
    [Range(0,100)]
    [SerializeField]
    private float integrity;//how intact it is. Material breaks apart when it reaches 0
    
    public List<GameObject> crackStages;
    private List<SpriteRenderer> crackSprites = new List<SpriteRenderer>();

    void Start()
    {
        foreach (GameObject crackStage in crackStages)
        {
            crackSprites.Add(crackStage.GetComponent<SpriteRenderer>());
        }
        integrity = maxIntegrity;
    }

    void Update()
    {
        setIntegrity(integrity);
    }

	void CollisionEnter2D(Collision2D coll)
    {
        HardMaterial hm = coll.gameObject.GetComponent<HardMaterial>();
        if (hm != null)
        {
            integrity -= hm.hardness / hardness * coll.relativeVelocity.magnitude;
            setIntegrity(integrity);
        }
    }

    private void setIntegrity(float newIntegrity)
    {
        integrity = newIntegrity;
        float baseAlpha = 1.0f - (integrity / maxIntegrity);
        for (int i = 0; i < crackSprites.Count; i++)
        {
            float thresholdLower = i * 1 / (float)crackSprites.Count;
            float alpha = 0;
            if (baseAlpha > thresholdLower)
            {
                alpha = (baseAlpha - thresholdLower) * crackSprites.Count;
            }
            SpriteRenderer sr = crackSprites[i];
            Color c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, alpha);
        }
    }

    public override SavableObject getSavableObject()
    {
        return new SavableObject(this, "integrity", integrity);
    }
    public override void acceptSavableObject(SavableObject savObj)
    {
        integrity = (float)savObj.data["integrity"];
        setIntegrity(integrity);
    }
}
