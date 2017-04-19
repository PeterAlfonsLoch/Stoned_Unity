﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameState
{
    public int id;
    public List<ObjectState> states = new List<ObjectState>();
    public ObjectState merky;//the object state in the list specifically for Merky

    public static int nextid = 0;
    public GameObject representation;//the player ghost that represents this game state

    //Instantiation
    public GameState() {
        id = nextid;
        nextid++;
    }
    public GameState(List<GameObject> list): this()
    {
        foreach (GameObject go in list){
            ObjectState os = new ObjectState(go);
            os.saveState();
            states.Add(os);
            if (go.name.Equals("merky"))
            {
                merky = os;
            }
        }
    }
    //Loading
    public void load()
    {
        foreach (ObjectState os in states)
        {
            os.loadState();
        }
    }
    public bool loadObject(GameObject go)
    {
        foreach (ObjectState os in states)
        {
            if (os.sceneName.Equals(go.scene.name) && os.objectName.Equals(go.name))
            {
                os.loadState();
                return true;
            }
        }
        return false;
    }
    //
    //Spawned Objects
    //

    //
    //Gets the list of the game objects that have object states in this game state
    //
    public List<GameObject> getGameObjects()
    {
        List<GameObject> objects = new List<GameObject>();
        foreach (ObjectState os in states)
        {
            objects.Add(os.getGameObject());
        }
        return objects;
    }
    //Returns true IFF the given GameObject has an ObjectState in this GameState
    public bool hasGameObject(GameObject go)
    {
        if (go == null)
        {
            return false;
        }
        foreach (ObjectState os in states)
        {
            if (os.objectName == go.name && os.sceneName == go.scene.name)
            {
                return true;
            }
        }
        return false;
    }
    //Representation (check point ghost)
    public void showRepresentation(GameObject ghostPrefab, int mostRecentId)
    {
        if (representation == null)
        {
            representation = GameObject.Instantiate(ghostPrefab);
            representation.transform.position = merky.position;
            representation.transform.localScale = merky.localScale;
            representation.transform.rotation = merky.rotation;
        }
        else
        {
            representation.SetActive(true);
        }
        //Set the Alpha Value
        SpriteRenderer sr = representation.GetComponent<SpriteRenderer>();
        Color c = sr.color;
        ParticleSystem ps = representation.GetComponentInChildren<ParticleSystem>();
                
        if (mostRecentId - id < 10)
        {
            sr.color = new Color(c.r, c.g, c.b, 1.0f);
            ps.Play();
        }
        else if (mostRecentId - id < 100)
        {
            sr.color = new Color(c.r, c.g, c.b, 0.9f);
            ps.Stop();
        }
        else
        {
            sr.color = new Color(c.r, c.g, c.b, 0.5f);
            ps.Stop();
        }
    }
    public bool checkRepresentation(Vector3 touchPoint)
    {
        return representation.GetComponent<SpriteRenderer>().bounds.Contains(touchPoint);
    }
    public void hideRepresentation()
    {
        if (representation != null)
        {
            representation.SetActive(false);
        }
    }
}
