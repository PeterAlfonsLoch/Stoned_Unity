using UnityEngine;
using System.Collections;

public class ObjectState {

    public Transform transformSave;
    public Vector2 velocity;
    public float angularVelocity;
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
        transformSave = go.transform;
        velocity = rb2d.velocity;
        angularVelocity = rb2d.angularVelocity;
    }
    public void loadState()
    {
        go = GameObject.Find(objectName);
        go.transform.localPosition = transformSave.position;
        go.transform.localScale = transformSave.localScale;
        go.transform.localRotation = transformSave.rotation;
        rb2d = go.GetComponent<Rigidbody2D>();
        rb2d.velocity = velocity;
        rb2d.angularVelocity = angularVelocity;
    }
}
