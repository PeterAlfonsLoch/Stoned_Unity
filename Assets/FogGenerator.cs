using UnityEngine;
using System.Collections;

public class FogGenerator : MonoBehaviour {

    public GameObject sampleCloud;
    public int maxNumberOfClouds = 100;

    // Use this for initialization
    void Start () {
        float range = 10f;
        float maxDiff = Vector3.Distance(new Vector3(0f, 0f), new Vector3(range, range));
        for (int i = 0; i < maxNumberOfClouds; i++)
        {
            GameObject newC = (GameObject)Instantiate(sampleCloud);
            newC.transform.position = sampleCloud.transform.position + new Vector3(Random.Range(-range, range), Random.Range(-range, range));
            float diff = Vector3.Distance(sampleCloud.transform.position, newC.transform.position);
            float alpha = 1f - (diff / maxDiff);
            newC.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
