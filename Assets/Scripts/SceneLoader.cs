using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour {

    public int sceneIndex;//the index of the scene to load
    private Scene scene;

	// Use this for initialization
	void Start () {
        scene = SceneManager.GetSceneAt(sceneIndex);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        loadLevel();
    }
    void loadLevel()
    {
        if ( ! scene.isLoaded)
        {
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
        }
    }
}
