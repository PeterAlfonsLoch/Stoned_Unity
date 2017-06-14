﻿using UnityEngine;

public abstract class MilestoneActivator : MemoryMonoBehaviour {

    public int incrementAmount = 1;
    public GameObject particle;
    public int starAmount = 25;
    public int starSpawnDuration = 25;
    public string abilityIndicatorName;
    
    public bool used = false;
    private float minX, maxX, minY, maxY;

    // Use this for initialization
    void Start()
    {
        if (transform.parent != null)
        {
            Bounds bounds = GetComponentInParent<SpriteRenderer>().bounds;
            float extra = 0.1f;
            minX = bounds.min.x - extra;
            maxX = bounds.max.x + extra;
            minY = bounds.min.y - extra;
            maxY = bounds.max.y + extra;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!used && coll.gameObject.Equals(GameManager.getPlayerObject()))
        {
            activate(true);
        }
    }

    public void activate(bool showFX)
    {
        if (showFX)
        {
            if (transform.parent != null)
            {
                sparkle();
            }
            if (abilityIndicatorName != null)
            {
                foreach (GameObject abilityIndicator in GameObject.FindGameObjectsWithTag("AbilityIndicator"))
                {
                    if (abilityIndicator.name.Contains(abilityIndicatorName))
                    {
                        AbilityGainEffect age = abilityIndicator.AddComponent<AbilityGainEffect>();
                        break;
                    }
                }
            }
        }
        used = true;
        activateEffect();
        GameManager.saveMemory(this);
        Destroy(this);//makes sure it can only be used once
    }

    public abstract void activateEffect();

    protected void sparkle()
    {//2016-03-17: copied from PlayerController.showTeleportStar(..)
        for (int i = 0; i < starAmount; i++)
        {
            GameObject newTS = (GameObject)Instantiate(particle);
            TeleportStarUpdater tsu = newTS.GetComponent<TeleportStarUpdater>();
            tsu.start = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
            tsu.waitTime = i*(starAmount/starSpawnDuration);
            tsu.position();
            tsu.turnOn(true);
        }
    }

    public override MemoryObject getMemoryObject()
    {
        return new MemoryObject(this, used);
    }
    public override void acceptMemoryObject(MemoryObject memObj)
    {
        if (memObj.found)
        {
            used = true;
            activate(false);
        }
    }
}
