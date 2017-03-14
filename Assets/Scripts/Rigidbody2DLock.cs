using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigidbody2DLock : SavableMonoBehaviour {
    //2017-01-31: this class is used to make RigidBody2D components kinematic on certain objects

    public List<GameObjectId> lockHolders = new List<GameObjectId>();//the objects that have a lock on this rb2d
    	
    public void lockRB2D()
    {
        Rigidbody2D rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;//lock rb2d
        rb2d.velocity = new Vector2(0, 0);
        rb2d.angularVelocity = 0;
    }

	public void addLock(GameObject lockHolder)
    {
        lockRB2D();
        lockHolders.Add(GameObjectId.createId(lockHolder));
    }

    public void removeLock(GameObject lockHolder)
    {
        for (int i = 0; i < lockHolders.Count; i++)
        {
            if (lockHolders[i].matches(lockHolder))
            {
                lockHolders.RemoveAt(i);
                break;
            }
        }
        if (lockHolders.Count == 0)
        {
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;//unlock rb2d
            Destroy(this);//take off lock altogether
        }
    }

    public bool holdsLock(GameObject lockHolder)
    {
        foreach (GameObjectId goId in lockHolders)
        {
            if (goId.matches(lockHolder))
            {
                return true;
            }
        }
        return false;
    }

    public override SavableObject getSavableObject()
    {
        return new Rigidbody2DLockSavable(this);
    }
}

//
//Class that saves the important variables of this class
//2017-02-07: copied from ShieldBubbleControllerSavable
//
public class Rigidbody2DLockSavable : SavableObject
{
    public List<GameObjectId> lockHolders = new List<GameObjectId>();

    public Rigidbody2DLockSavable() { }//only called by the method that reads it from the file
    public Rigidbody2DLockSavable(Rigidbody2DLock rb2dl)
    {
        saveState(rb2dl);
    }

    public override System.Type getSavableMonobehaviourType()
    {
        return typeof(Rigidbody2DLock);
    }
    public override void addScript(GameObject go)
    {
        Rigidbody2DLock rb2dl = go.AddComponent<Rigidbody2DLock>();
        rb2dl.lockRB2D();
    }

    public override void loadState(GameObject go)
    {
        Rigidbody2DLock rb2dl = go.GetComponent<Rigidbody2DLock>();
        if (rb2dl != null)
        {
            rb2dl.lockHolders = this.lockHolders;
        }
    }
    public override void saveState(SavableMonoBehaviour smb)
    {
        Rigidbody2DLock rb2dl = (Rigidbody2DLock)smb;
        this.lockHolders = rb2dl.lockHolders;
    }
}