﻿using UnityEngine;
using System.Collections;

public class TeleportRangeIndicatorUpdater : MonoBehaviour {

    public GameObject parentObj;

    private PlayerController controller;
    private float baseWidth=0;
    private float baseHeight=0;
    private float baseRange = 2.5f;//2017-01-30: got this measurement from a test run. If the sprite size ever changes, this value will also have to change
    private float baseScale = 1;
    SpriteRenderer sr;

    // Use this for initialization
    void Start()
    {
        if (parentObj != null)
        {
            controller = parentObj.GetComponent<PlayerController>();
        }
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        Debug.Log("TRIU: range: " + baseRange + ", scale: " + baseScale);
    }
    
    public void updateRange()
    {
        if (controller.teleportTime <= Time.time)
        {
            sr.enabled = true;
        }
        else
        {
            sr.enabled = false;
        }
        float newSize = controller.range;
        setRange(newSize);
    }
    public void setRange(float range)
    {
        float newScale = baseScale * range / baseRange;
        Vector3 newV = new Vector3(newScale, newScale, 0);
        transform.localScale = newV;
    }
}
