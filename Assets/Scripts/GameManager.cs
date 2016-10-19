using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameManager : MonoBehaviour {
    public bool save = false;
    public bool load = false;
    public int chosenId = 0;
    public  int amount = 0;
    private static List<GameState> gameStates = new List<GameState>();
    private static List<GameObject> gameObjects = new List<GameObject>();

    // Use this for initialization
    void Start() {
        foreach (Rigidbody2D rb in FindObjectsOfType<Rigidbody2D>()) {
            gameObjects.Add(rb.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
	if (save == true)
        {
            save = false;
            Save();
        }
        if (load == true)
        {
            load = false;
            Load(chosenId);
        }
    }

    public  void Save()
    {
        gameStates.Add(new GameState(gameObjects));
        amount++;
    }
    public  void Load(int gamestateId)
    {
        gameStates[gamestateId].load();
    }
    public static void saveToFile()
    {
        ES2.Save(gameStates, "merky.txt");
    }
    public static void loadFromFile()
    {
        gameStates = ES2.Load<List<GameState>>("merky.txt");
    }
}


