using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState
{
    public int id;
    public List<ObjectState> states = new List<ObjectState>();
    public ObjectState merky;//the object state in the list specifically for Merky

    private static int nextid = 0;
    private GameObject representation;//the player ghost that represents this game state

    public GameState() {
        id = nextid;
        nextid++;
    }
    public GameState(List<GameObject> list): this()
    {
        foreach (GameObject go in list){
            ObjectState os = new ObjectState(go.name);
            states.Add(os);
            if (go.name.Equals("merky"))
            {
                merky = os;
            }
        }
    }
    public void load()
    {
        foreach (ObjectState os in states)
        {
            os.loadState();
        }
    }
    public void showRepresentation(GameObject ghostPrefab)
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
    }
    public bool checkRepresentation(Vector3 touchPoint)
    {
        return representation.GetComponent<SpriteRenderer>().bounds.Contains(touchPoint);
    }
    public void hideRepresentation()
    {
        representation.SetActive(false);
    }
}
