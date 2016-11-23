
using UnityEngine;

public class SavableObject{

    //this class is the parent class of all scripts that have data to be saved
    
    public SavableObject(){}

    public virtual void loadState(GameObject go) { }

    public virtual void saveState(SavableMonoBehaviour go) { }
}
