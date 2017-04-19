﻿using System.Collections;
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
        GameManager.destroyObject(gameObject);
    }

    public override MemoryObject getMemoryObject()
    {
        return new MemoryObject(this, discovered);
    }
    public override void acceptMemoryObject(MemoryObject memObj)
    {
        if (memObj.found)
        {
            previouslyDiscovered();
        }
    }
}
