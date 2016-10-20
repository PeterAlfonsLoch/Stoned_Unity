using UnityEngine;
using System.Collections;

public class RewindGestureProfile: GestureProfile
{
    private GameManager gm;
    public RewindGestureProfile()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }
    
    public override void processTapGesture(GameObject go)
    {
    }
    public override void processTapGesture(Vector3 curMPWorld)
    {
        gm.processTapGesture(curMPWorld);
    }
    public override void processHoldGesture(Vector3 curMPWorld, float holdTime, bool finished)
    {
    }
}
