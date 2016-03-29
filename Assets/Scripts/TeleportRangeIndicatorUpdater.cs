using UnityEngine;
using System.Collections;

public class TeleportRangeIndicatorUpdater : MonoBehaviour {

    public GameObject parentObj;

    private PlayerController controller;
    private float baseWidth=0;
    private float baseHeight=0;
    SpriteRenderer sr;
    float size = 0;

    // Use this for initialization
    void Start()
    {
        if (parentObj != null)
        {
            controller = parentObj.GetComponent<PlayerController>();
        }
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        Vector3 bsize = sr.bounds.size;
        baseWidth = bsize.x;
        baseHeight = bsize.y;
        if (size > 0)
        {
            setSize(size);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //}
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
        //transform.position = parentObj.transform.position;
        //transform.localScale = new Vector3(1, 1);
        float newSize = controller.range * 2;
        setSize(newSize);
    }
    public void setSize(float newSize) {
        //if (Mathf.Abs(baseWidth - newSize) > 1)
        //{
        size = newSize;
        if (baseWidth > 0 && baseHeight > 0)
        {
            Vector3 newV = new Vector3(newSize / baseWidth, newSize / baseHeight, 0);
            //if (Mathf.Abs(transform.localScale.magnitude - newV.magnitude) > 1) {
            transform.localScale = newV;
        }
        // }
        //}
        //transform.localScale.y = newSize / height;
        //GetComponent<SpriteRenderer>().sprite.bounds.SetMinMax(controller.range * 2, controller.range * 2);
    }

    //void LastUpdate()
    //{
    //    //transform.position = parentObj.transform.position;
    //    //Vector3 size = GetComponent<SpriteRenderer>().bounds.size;
    //    //float width = size.x;
    //    //float height = size.y;
    //    float newSize = controller.range * 2;
    //    transform.localScale = new Vector3(1, 1) * newSize;// 100, 100);// newSize / width, newSize / height);
    //    //transform.localScale.y = newSize / height;
    //    //GetComponent<SpriteRenderer>().sprite.bounds.SetMinMax(controller.range * 2, controller.range * 2);

    //}
}
