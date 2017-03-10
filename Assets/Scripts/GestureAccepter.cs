using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Place this on a common world object to allow it to accept a gesture
/// </summary>
public class GestureAccepter : MonoBehaviour {

    public virtual bool acceptsTapGesture() { return false; }
    public virtual bool acceptsHoldGesture() { return false; }

    public virtual void processTapGesture() { }
    public virtual void processHoldGesture(bool finished) { }
}
