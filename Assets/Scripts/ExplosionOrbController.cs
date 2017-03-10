using UnityEngine;
using System.Collections;

public class ExplosionOrbController : MonoBehaviour {
    public int forceAmount = 300;
    private CircleCollider2D cc2D;
    private ForceTeleportAbility fta;
    private RaycastHit2D[] rh2ds = new RaycastHit2D[1];//used in Update() for a method call. Not actually updated
    
    private float chargeTime = 0.0f;//amount of time spent charging

    //Tutorial toggles
    public bool explodesUponContact = true;//false = requires mouseup (tap up) to explode
    public bool chargesAutomatically = true;//false = requires hold to charge
    public bool explodesAtAll = true;//false = doesn't do anything

    // Use this for initialization
    void Start () {
        cc2D = GetComponent<CircleCollider2D>();
        fta = GetComponent<ForceTeleportAbility>();
    }

    // Update is called once per frame
    void Update()
    {
        if (explodesAtAll)
        {
            if (explodesUponContact && cc2D.Cast(Vector2.zero, rh2ds, 0, true) > 0 && !rh2ds[0].collider.isTrigger)
            {
                Debug.Log("Collision!");
                if (chargeTime >= fta.maxHoldTime)
                {
                    fta.processHoldGesture(transform.position, chargeTime, true);
                    chargeTime = 0;
                }
            }
            if (chargesAutomatically)
            {
                chargeTime += Time.deltaTime;
                fta.processHoldGesture(transform.position, chargeTime, false);
            }
        }
    }

}
