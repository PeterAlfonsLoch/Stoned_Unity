using UnityEngine;
using System.Collections;

public class ExplosionOrbController : MonoBehaviour {
    public int forceAmount = 300;
    private CircleCollider2D cc2D;
    private ForceTeleportAbility fta;
    private RaycastHit2D[] rh2ds = new RaycastHit2D[1];//used in Update() for a method call. Not actually updated

    private float timeSinceLastExplosion = 0.0f;

    // Use this for initialization
    void Start () {
        cc2D = GetComponent<CircleCollider2D>();
        fta = GetComponent<ForceTeleportAbility>();
    }
	
	// Update is called once per frame
	void Update () {
        if (cc2D.Cast(Vector2.zero, rh2ds, 0, true) > 0)
        {
            Debug.Log("Collision!");
            if (Time.time - timeSinceLastExplosion > fta.maxHoldTime)
            {
                fta.processHoldGesture(transform.position, Time.time - timeSinceLastExplosion, true);
                timeSinceLastExplosion = Time.time;
            }
        }
        else
        {
            fta.processHoldGesture(transform.position, Time.time - timeSinceLastExplosion, false);
        }
    }

}
