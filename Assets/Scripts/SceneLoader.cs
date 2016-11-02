using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour {

    public int sceneIndex;//the index of the scene to load
    private bool isLoaded = false;

	// Use this for initialization
	void Start () {
	
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
        if (!isLoaded)
        {
            isLoaded = true;
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
        }
    }
}
