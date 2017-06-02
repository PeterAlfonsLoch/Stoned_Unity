using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardMaterial : SavableMonoBehaviour {

    public static float MINIMUM_CRACKSOUND_THRESHOLD = 1.0f;//the minimum percent of damage done to make a sound

    public float hardness = 1.0f;
    public float maxIntegrity = 100f;
    [Range(0,100)]
    [SerializeField]
    private float integrity;//how intact it is. Material breaks apart when it reaches 0
    public GameObject crackedPrefab;//the prefab for the object broken into pieces
    public List<GameObject> crackStages;
    private List<SpriteRenderer> crackSprites = new List<SpriteRenderer>();
    public List<AudioClip> crackSounds;

    public Shattered shattered;

    void Start()
    {
        foreach (GameObject crackStage in crackStages)
        {
            crackSprites.Add(crackStage.GetComponent<SpriteRenderer>());
        }
        if (integrity == 0)
        {
            setIntegrity(maxIntegrity);
        }
        else
        {//show cracks from the start
            setIntegrity(integrity);
        }
    }

    //void Update()
    //{
    //    setIntegrity(integrity);
    //}

    void OnCollisionEnter2D(Collision2D coll)
    {
        HardMaterial hm = coll.gameObject.GetComponent<HardMaterial>();
        if (hm != null)
        {
            float hitHardness = hm.hardness / hardness * coll.relativeVelocity.magnitude;
            addIntegrity(-1 * hitHardness);
            float hitPercentage = hitHardness * 100 / maxIntegrity;
            for (int i = crackSounds.Count - 1; i >= 0; i--)
            {
                float crackThreshold = 100 / (crackSprites.Count + 1 - i) - 20;
                if (i == 0)
                {
                    crackThreshold = MINIMUM_CRACKSOUND_THRESHOLD;
                }
                if (hitPercentage > crackThreshold)
                {
                    AudioSource.PlayClipAtPoint(crackSounds[i], coll.contacts[0].point,hitPercentage/400+0.75f);
                    break;
                }
            }
        }
    }

    public bool isIntact()
    {
        return integrity > 0;
    }

    public void addIntegrity(float addend)
    {
        setIntegrity(integrity + addend);
    }

    private void setIntegrity(float newIntegrity)
    {
        float oldIntegrity = integrity;
        integrity = Mathf.Clamp(newIntegrity, 0, maxIntegrity);
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
        else if (oldIntegrity > 0)
        {
            GameObject pieces = Instantiate(crackedPrefab);
            pieces.transform.position = transform.position;
            pieces.transform.rotation = transform.rotation;
            pieces.transform.localScale = transform.localScale;
            pieces.name += System.DateTime.Now.Ticks;
            GameManager.refresh();
            gameObject.SetActive(false);
            GameManager.saveScab();
            if (shattered != null)
            {
                shattered();//call delegate method
            }
        }
    }

    public float getIntegrity()
    {
        return integrity;
    }

    /// <summary>
    /// Gets called when integrity reaches 0
    /// </summary>
    public delegate void Shattered();

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
