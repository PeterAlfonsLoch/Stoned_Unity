using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public string sceneName;

    private Rigidbody2D rb2d;
    private GameObject go;

    public ObjectState() { }
    public ObjectState(GameObject goIn)
    {
        go = goIn;
        objectName = go.name;
        sceneName = go.scene.name;
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
        if (go == null)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.isLoaded)
            {
                foreach (GameObject sceneGo in scene.GetRootGameObjects())
                {
                    foreach(Transform childTransform in sceneGo.GetComponentsInChildren<Transform>())
                    {
                        GameObject childGo = childTransform.gameObject;
                        if (childGo.name.Equals(objectName))
                        {
                            this.go = childGo;
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }
        go.transform.position = position;
        go.transform.localScale = localScale;
        go.transform.rotation = rotation;
        rb2d = go.GetComponent<Rigidbody2D>();
        rb2d.velocity = velocity;
        rb2d.angularVelocity = angularVelocity;
    }
}
