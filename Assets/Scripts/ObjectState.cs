﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    public List<SavableObject> soList;
    public static SavableObject dummySO = new SavableObject();//for objects that don't have them
    //Name
    public string objectName;
    public string sceneName;

    private Rigidbody2D rb2d;
    private GameObject go;
    private List<SavableMonoBehaviour> smbList;

    public ObjectState() { }
    public ObjectState(GameObject goIn)
    {
        go = goIn;
        objectName = go.name;
        sceneName = go.scene.name;
        rb2d = go.GetComponent<Rigidbody2D>();
        smbList = new List<SavableMonoBehaviour>();
        smbList.AddRange(go.GetComponents<SavableMonoBehaviour>());
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
        soList = new List<SavableObject>();
        foreach (SavableMonoBehaviour smb in smbList)
        {
            this.soList.Add(smb.getSavableObject());
        }
    }
    public void loadState()
    {
        getGameObject();//finds and sets the game object
        if (go == null)
        {
            return;//don't load the state if go is null
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
        foreach (SavableObject so in this.soList)
        {
            if (go.GetComponent(so.getSavableMonobehaviourType()) == null)
            {
                so.addScript(go);
            }
            so.loadState(go);
        }
    }

    //
    //Retrieves the variable, go,
    //and if go is null, it finds the correct GameObject and sets go
    //
    public GameObject getGameObject()
    {
        if (go == null || ReferenceEquals(go, null))//2016-11-20: reference equals test copied from an answer by sindrijo: http://answers.unity3d.com/questions/13840/how-to-detect-if-a-gameobject-has-been-destroyed.html
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.IsValid() && scene.isLoaded)
            {
                foreach (GameObject sceneGo in scene.GetRootGameObjects())
                {
                    if (sceneGo.name.Equals(objectName))
                    {
                        this.go = sceneGo;
                    }
                    else {
                        foreach (Transform childTransform in sceneGo.GetComponentsInChildren<Transform>())
                        {
                            GameObject childGo = childTransform.gameObject;
                            if (childGo.name.Equals(objectName))
                            {
                                this.go = childGo;
                            }
                        }
                    }
                }
                //if not found, do stuff
                if (go == null || ReferenceEquals(go, null))
                {
                    //if is spawned object, make it
                    foreach (SavableObject so in soList)
                    {
                        if (so.isSpawnedObject)
                        {
                            go = so.spawnObject();
                            go.name = this.objectName;
                            GameManager.addObject(go);
                            break;
                        }
                    }
                }
            }
            else
            {
                go = null;
            }
        }
        return go;
    }
}
