
using System.Collections.Generic;
using UnityEngine;

public class SavableObject
{

    //this class stores variables that need to be saved from MonoBehaviours

    public Dictionary<string, System.Object> data = new Dictionary<string, System.Object>();
    /// <summary>
    /// True if it's an object that spawned during runtime
    /// </summary>
    public bool isSpawnedObject;

    public SavableObject() { }

    /// <summary>
    /// Spawn this saved object's game object
    /// This method is used during load
    /// precondition: the game object does not already exist (or at least has not been found)
    /// </summary>
    /// <returns></returns>
    public virtual GameObject spawnObject()
    {
        throw new System.MissingMethodException("This method is not supported in this subclass: " + GetType().Name);
    }

    public virtual System.Type getSavableMonobehaviourType()
    {
        return typeof(SavableMonoBehaviour);
    }

    ///<summary>
    ///Adds this SavableObject's SavableMonobehaviour to the given GameObject
    ///and sets the variables.
    ///</summary>
    ///<param name="go">The GameObject to add the script to</param>
    public virtual void addScript(GameObject go)
    {
        throw new System.MissingMethodException("This method is not supported in this subclass: " + GetType().Name);
    }

    public virtual void loadState(GameObject go) { }

    public virtual void saveState(SavableMonoBehaviour go) { }
}
