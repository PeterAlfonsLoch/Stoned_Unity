using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SavableMonoBehaviour: MonoBehaviour {
    /// <summary>
    /// "Save": Returns the SavableObject used to save this object's configuration state
    /// </summary>
    /// <returns></returns>
    public abstract SavableObject getSavableObject();
    /// <summary>
    /// "Load": replaces its current state with the state in the given SavableObject
    /// </summary>
    /// <param name="savObj"></param>
    public abstract void acceptSavableObject(SavableObject savObj);
}
