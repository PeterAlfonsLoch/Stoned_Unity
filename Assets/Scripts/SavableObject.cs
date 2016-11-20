
using UnityEngine;

public abstract class SavableObject{

    //this class is the parent class of all scripts that have data to be saved

    public SavableObject(){}

    public abstract void loadState(GameObject go);

    public abstract void saveState(SavableMonoBehaviour go);
}
