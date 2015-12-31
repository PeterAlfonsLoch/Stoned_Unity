using UnityEngine;
using System.Collections;

public class TeleportRangeIndicatorUpdater : MonoBehaviour {

    public GameObject parentObj;

    private PlayerController controller;
    private float baseWidth=0;
    private float baseHeight=0;

	// Use this for initialization
	void Start () {
        controller = parentObj.GetComponent<PlayerController>();
            Vector3 size = GetComponent<SpriteRenderer>().bounds.size;
            baseWidth = size.x;
            baseHeight = size.y;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void updateRange() { 
        //transform.position = parentObj.transform.position;
        //transform.localScale = new Vector3(1, 1);
        float newSize = (float)controller.range * 2;
        //if (Mathf.Abs(baseWidth - newSize) > 1)
        //{
            Vector3 newV = new Vector3(newSize / baseWidth, newSize / baseHeight);
            //if (Mathf.Abs(transform.localScale.magnitude - newV.magnitude) > 1) {
            transform.localScale = newV;
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
