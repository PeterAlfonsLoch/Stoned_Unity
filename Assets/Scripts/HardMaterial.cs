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
    public GameObject crackedPrefab;//the prefab for the object broken into pieces
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

	void OnCollisionEnter2D(Collision2D coll)
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
        if (integrity > 0) {
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
            gameObject.SetActive(true);
        }
        else
        {
            GameObject pieces = Instantiate(crackedPrefab);
            pieces.transform.position = transform.position;
            pieces.transform.rotation = transform.rotation;
            pieces.transform.localScale = transform.localScale;
            GameManager.refresh();
            gameObject.SetActive(false);
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
