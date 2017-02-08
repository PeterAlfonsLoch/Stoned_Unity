
using UnityEngine;

public class SavableObject{

    //this class is the parent class of all scripts that have data to be saved
    
    public SavableObject(){}

    public virtual bool isSpawnedObject()//whether or not this object is part of the level or spawned by the player or other entity
    {
        return false;
    }
    //
    //Spawn this saved object's game object
    //This method is used during load
    //precondition: the game object does not already exist (or at least has not been found)
    //
    public virtual GameObject spawnObject()
    {
        throw new System.MissingMethodException("This method is not supported in this subclass: "+GetType().Name);
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
