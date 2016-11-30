using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenArea : MemoryMonoBehaviour {
    //The class is just here so that the Hidden Areas themselves
    //remember whether they've been found or not,
    //and not their triggers

    public bool discovered = false;

    //2016-11-26: called when this HiddenArea has just been discovered now
	public void nowDiscovered()
    {
        discovered = true;
        GameManager.saveMemory(this);
        gameObject.AddComponent<Fader>();
    }
	
    //2016-11-26: called when this HiddenArea had been discovered in a previous session
	public void previouslyDiscovered()
    {
        Destroy(gameObject);
    }

    public override MemoryObject getMemoryObject()
    {
        return new HiddenAreaMemory(this);
    }
}

//
//Class that saves the important variables of this class
//
public class HiddenAreaMemory : MemoryObject
{
    public HiddenAreaMemory() { }//only called by the method that reads it from the file
    public HiddenAreaMemory(HiddenArea ha) : base(ha)
    {
        saveState(ha);
    }

    public override void loadState(GameObject go)
    {
        HiddenArea ha = go.GetComponent<HiddenArea>();
        if (ha != null)
        {
            if (this.found)
            {
                ha.previouslyDiscovered();
            }
        }
    }
    public override void saveState(MemoryMonoBehaviour mmb)
    {
        HiddenArea ha = ((HiddenArea)mmb);
        this.found = ha.discovered;
    }
}
