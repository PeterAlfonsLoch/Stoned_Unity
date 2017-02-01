using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigidbody2DLock : MonoBehaviour {
    //2017-01-31: this class is used to make RigidBody2D components kinematic on certain objects

    private List<GameObject> lockHolders = new List<GameObject>();//the objects that have a lock on this rb2d
    	
	public void addLock(GameObject lockHolder)
    {
        Rigidbody2D rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;//lock rb2d
        rb2d.velocity = new Vector2(0, 0);
        lockHolders.Add(lockHolder);
    }

    public void removeLock(GameObject lockHolder)
    {
        lockHolders.Remove(lockHolder);
        if (lockHolders.Count == 0)
        {
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;//unlock rb2d
            Destroy(this);//take off lock altogether
        }
    }

    public bool holdsLock(GameObject lockHolder)
    {
        return lockHolders.Contains(lockHolder);
    }
}
