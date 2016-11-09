using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{

    public string sceneName;//the index of the scene to load
    private PolygonCollider2D playerColl;
    private bool isLoaded = false;
    private BoxCollider2D bc;

    // Use this for initialization
    void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
        foreach (GameObject go in SceneManager.GetSceneAt(0).GetRootGameObjects())
        {
            if (go.tag.Equals("Player"))
            {
                playerColl = go.GetComponent<PolygonCollider2D>();
                break;
            }
        }
    }

    void Update()
    {

        if (!isLoaded && playerColl.bounds.Intersects(bc.bounds))
        {
            isLoaded = true;
            loadLevel();
        }
        if (isLoaded && !playerColl.bounds.Intersects(bc.bounds))
        {
            isLoaded = false;
            unloadLevel();
        }
    }
    void loadLevel()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    void unloadLevel()
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
