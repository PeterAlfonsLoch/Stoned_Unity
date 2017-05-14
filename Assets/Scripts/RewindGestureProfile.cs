using UnityEngine;
using System.Collections;

public class RewindGestureProfile: GestureProfile
{
    public override void processTapGesture(GameObject go)
    {
    }
    public override void processTapGesture(Vector3 curMPWorld)
    {
        gm.processTapGesture(curMPWorld);
    }
    public override void processHoldGesture(Vector3 curMPWorld, float holdTime, bool finished)
    {
        if (finished)
        {
            gm.processTapGesture(curMPWorld);
            GameObject.FindObjectOfType<GestureManager>().adjustHoldThreshold(holdTime);
        }
    }
    public override void processPinchGesture(int adjustment)
    {
        cmaController.adjustScalePoint(adjustment);
        //GestureProfile switcher
        if (cmaController.getScalePointIndex() < CameraController.SCALEPOINT_TIMEREWIND
            && plrController.isIntact())
        {
            gestureManager.switchGestureProfile("Main");
            gm.hidePlayerGhosts();
        }
    }
}
