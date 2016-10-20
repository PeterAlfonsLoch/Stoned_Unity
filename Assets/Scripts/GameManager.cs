using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameManager : MonoBehaviour
{
    public bool save = false;
    public bool load = false;
    public int chosenId = 0;
    public int amount = 0;
    public GameObject playerGhost;//this is to show Merky in the past
    private static List<GameState> gameStates = new List<GameState>();
    private static List<GameObject> gameObjects = new List<GameObject>();

    private static GameManager instance;
    private CameraController camCtr;

    // Use this for initialization
    void Start()
    {
        refreshGameObjects();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            instance.addAll(gameObjects);
            Destroy(gameObject);
        }

        camCtr = FindObjectOfType<CameraController>();
    }
    public void addAll(List<GameObject> list)
    {
        gameObjects.AddRange(list);
    }

    // Update is called once per frame
    void Update()
    {
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

    public void refreshGameObjects()
    {
        gameObjects = new List<GameObject>();
        foreach (Rigidbody2D rb in FindObjectsOfType<Rigidbody2D>())
        {
            gameObjects.Add(rb.gameObject);
        }
    }

    public void Save()
    {
        gameStates.Add(new GameState(gameObjects));
        amount++;
    }
    public void Load(int gamestateId)
    {
        gameStates[gamestateId].load();
        for (int i = gameStates.Count - 1; i > gamestateId ; i--)
        {
            Destroy(gameStates[i].representation);
            gameStates.RemoveAt(i);
        }
        GameState.nextid = gamestateId + 1;
    }
    public static void saveToFile()
    {
        ES2.Save(gameStates, "merky.txt");
    }
    public static void loadFromFile()
    {
        gameStates = ES2.LoadList<GameState>("merky.txt");
    }

    //void Awake()
    //{
    //    if (ES2.Exists("merky.txt"))
    //    {
    //        loadFromFile();
    //        Load(gameStates.Count - 1);
    //        CameraController cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    //        cam.pinPoint();
    //        cam.recenter();
    //        cam.refocus();
    //    }
    //}
    void OnApplicationQuit()
    {
        Save();
        saveToFile();
    }

    public void showPlayerGhosts()
    {
        foreach (GameState gs in gameStates)
        {
            gs.showRepresentation(playerGhost);
        }
    }
    public void hidePlayerGhosts()
    {
        foreach (GameState gs in gameStates)
        {
            gs.hideRepresentation();
        }
    }
    public void processTapGesture(Vector3 curMPWorld)
    {
        Debug.Log("GameManager.pTG: curMPWorld: " + curMPWorld);
        GameState final = null;
        foreach (GameState gs in gameStates)
        {
            if (gs.checkRepresentation(curMPWorld))
            {
                if (final == null || gs.id > final.id)//assuming the later ones have higher id values
                {
                    final = gs;//keep the latest one
                }
            }
        }
        if (final != null)
        {
            hidePlayerGhosts();
            Load(final.id);
            camCtr.adjustScalePoint(-1);
            camCtr.recenter();
            camCtr.refocus();
        }
    }
}


