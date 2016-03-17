using UnityEngine;
using System.Collections;

public class MilestoneActivator : MonoBehaviour {

    public int incrementAmount = 1;
    public GameObject particle;
    public int starAmount = 25;
    public int starSpawnDuration = 25;

    protected GameObject playerObject;
    protected bool used = false;
    private float minX, maxX, minY, maxY;

    // Use this for initialization
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        Bounds bounds = GetComponentInParent<SpriteRenderer>().bounds;
        float extra = 0.1f;
        minX = bounds.min.x - extra;
        maxX = bounds.max.x + extra;
        minY = bounds.min.y - extra;
        maxY = bounds.max.y + extra;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if ( ! used && coll.gameObject.Equals(playerObject))
        {
            sparkle();
            used = true;
            playerObject.GetComponent<PlayerController>().maxAirPorts += incrementAmount;
            Destroy(this);//makes sure it can only be used once
        }
    }

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
}
