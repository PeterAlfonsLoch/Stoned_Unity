using UnityEngine;
using System.Collections;

public class ObjectState {
    
    //Transform
    public Vector3 position;
    public Vector3 localScale;
    public Quaternion rotation;
    //RigidBody2D
    public Vector2 velocity;
    public float angularVelocity;
    //Name
    public string objectName;

    private Rigidbody2D rb2d;
    private GameObject go;

    public ObjectState() { }
    public ObjectState (string name) {
        objectName = name;
        go = GameObject.Find(name);
        rb2d = go.GetComponent<Rigidbody2D>();
        saveState();
	}

    public void saveState()
    {
        position = go.transform.position;
        localScale = go.transform.localScale;
        rotation = go.transform.rotation;
        velocity = rb2d.velocity;
        angularVelocity = rb2d.angularVelocity;
    }
    public void loadState()
    {
        go = GameObject.Find(objectName);
        go.transform.position = position;
        go.transform.localScale = localScale;
        go.transform.rotation = rotation;
        rb2d = go.GetComponent<Rigidbody2D>();
        rb2d.velocity = velocity;
        rb2d.angularVelocity = angularVelocity;
    }
}
