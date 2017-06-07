using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedPiece : SavableMonoBehaviour {
    //2017-05-02: the script to make broken pieces work with time rewind system
    //Apply to parent object of broken object prefab

    public string prefabName;
    
    public override bool isSpawnedObject()
    {
        return true;
    }
    public override string getPrefabName()
    {
        return prefabName;
    }

    public override SavableObject getSavableObject()
    {
        return new SavableObject(this);
    }

    public override void acceptSavableObject(SavableObject savObj)
    {
    }
}
