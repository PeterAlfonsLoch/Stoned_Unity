using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState
{
    int id;
    public List<ObjectState> states = new List<ObjectState>();

    private static int nextid = 1;

    public GameState() {
        id = nextid;
        nextid++;
    }
    public GameState(List<GameObject> list): this()
    {
        foreach (GameObject go in list){
            states.Add(new ObjectState(go.name));
        }
    }
    public void load()
    {
        foreach (ObjectState os in states)
        {
            os.loadState();
        }
    }
}
