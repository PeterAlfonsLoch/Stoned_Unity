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
        return new SavableObject("Rigidbody2DLock","lockHolders", lockHolders);
    }
    public override void acceptSavableObject(SavableObject savObj)
    {
        lockHolders = (List<GameObjectId>)savObj.data["lockHolders"];
    }
}