using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBubbleController : MonoBehaviour {

    public float range = 3;//how big the shield is
    private float baseWidth = 0;
    private float baseHeight = 0;

    // Use this for initialization
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector3 bsize = sr.bounds.size;
        baseWidth = bsize.x;
        baseHeight = bsize.y;
    }

    // Update is called once per frame
    //void Update () {
    //}

    public void setRange(float newRange)
    {
        this.range = newRange;
        if (baseWidth > 0 && baseHeight > 0)
        {
            Vector3 newV = new Vector3(newRange / baseWidth, newRange / baseHeight, 0);
            transform.localScale = newV;
        }
    }
}
