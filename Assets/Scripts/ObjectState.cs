using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ObjectState
{

    //Transform
    public Vector3 position;
    public Vector3 localScale;
    public Quaternion rotation;
    //RigidBody2D
    public Vector2 velocity;
    public float angularVelocity;
    //Saveable Object
    public SavableObject so;
    public static SavableObject dummySO = new SavableObject();//for objects that don't have them
    //Name
    public string objectName;
    public string sceneName;

    private Rigidbody2D rb2d;
    private GameObject go;
    private SavableMonoBehaviour smb;

    public ObjectState() { }
    public ObjectState(GameObject goIn)
    {
        go = goIn;
        objectName = go.name;
        sceneName = go.scene.name;
        rb2d = go.GetComponent<Rigidbody2D>();
        smb = go.GetComponent<SavableMonoBehaviour>();
    }

    public void saveState()
    {
        position = go.transform.position;
        localScale = go.transform.localScale;
        rotation = go.transform.rotation;
        if (rb2d != null)
        {
            velocity = rb2d.velocity;
            angularVelocity = rb2d.angularVelocity;
        }
        if (smb != null)
        {
            this.so = smb.getSavableObject();
        }
        else
        {
            this.so = dummySO;
        }
    }
    public void loadState()
    {
        if (go == null || !ReferenceEquals(go, null))//2016-11-20: reference equals test copied from an answer by sindrijo: http://answers.unity3d.com/questions/13840/how-to-detect-if-a-gameobject-has-been-destroyed.html
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.IsValid() && scene.isLoaded)
            {
                foreach (GameObject sceneGo in scene.GetRootGameObjects())
                {
                    foreach (Transform childTransform in sceneGo.GetComponentsInChildren<Transform>())
                    {
                        GameObject childGo = childTransform.gameObject;
                        if (childGo.name.Equals(objectName))
                        {
                            this.go = childGo;
                        }
                    }
                }
                //if not found, do stuff
                if (go == null || !ReferenceEquals(go, null))
                {
                    //if is spawned object, make it
                    if (so.isSpawnedObject())
                    {
                        go = so.spawnObject();
                        go.name = this.objectName;
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
        if (rb2d != null)
        {
            rb2d.velocity = velocity;
            rb2d.angularVelocity = angularVelocity;
        }
        if (this.so != null)
        {
            this.so.loadState(go);
        }
    }
}
