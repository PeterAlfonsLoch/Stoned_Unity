using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameManager : MonoBehaviour {
    public static List<GameObject> savedGames = new List<GameObject>();
    public GameObject root;
    public bool save = false;
    public bool load = false;
    private static List<GameState> gameStates = new List<GameState>();

    private static Rigidbody2D rb2d;

    // Use this for initialization
    void Start () {
        rb2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	if (save == true)
        {
            save = false;
            Save(root);
        }
        if (load == true)
        {
            load = false;
            Load(root);
        }
    }

    public static void Save(GameObject root)
    {
        savedGames.Add(root);
        //BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        file.Close();
        //bf.Serialize(file, savedGames);
        //RSManager.Serialize<GameObject>(root);
        GameState gs = new GameState(root.transform, rb2d.velocity, rb2d.angularVelocity);
        gameStates.Add(gs);
        ES2.Save(gameStates, "merky.txt");
        //ES2.Save(root.transform,"merky.txt?tag=transform");
        ////ES2.Save(root.transform, "merky.txt?tag=transform2");
        //ES2.Save(rb2d.velocity, "merky.txt?tag=rb2dvelocity");
        //ES2.Save(rb2d.angularVelocity, "merky.txt?tag=rb2dangularvelocity");

    }
    public static void Load(GameObject root)
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            //savedGames = (List<GameObject>)bf.Deserialize(file);
            //file.Close();
            //ES2.Load<Transform>("merky.txt?tag=transform", root.transform);
            ////ES2.Load<Transform>("merky.txt?tag=transform2", root.transform);
            //rb2d.velocity = ES2.Load<Vector2>("merky.txt?tag=rb2dvelocity");
            //rb2d.angularVelocity = ES2.Load<float>("merky.txt?tag=rb2dangularvelocity");
            GameState gs = ES2.Load<GameState>("merky.txt");
            root.transform.position = gs.transform.position;
            root.transform.localScale = gs.transform.localScale;
            root.transform.rotation = gs.transform.rotation;
            rb2d.velocity = gs.velocity;
            rb2d.angularVelocity = gs.angularVelocity;

        }
    }
}

public class GameState
{
    public Transform transform;
    public Vector2 velocity;
    public float angularVelocity;
    public GameState() {}
    public GameState(Transform t, Vector2 v, float av)
    {
        transform = t;
        velocity = v;
        angularVelocity = av;
    }
}
