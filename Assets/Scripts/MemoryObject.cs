
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryObject
{

    //this class is the parent class of all scripts that have memories to be saved

    public bool found = false;//whether this memory has been obtained by Merky yet
    public string objectName;
    public string sceneName;

    public MemoryObject() { }

    public MemoryObject(MemoryMonoBehaviour mmb)
    {
        objectName = mmb.gameObject.name;
        sceneName = mmb.gameObject.scene.name;
    }

    public void loadState() {
        GameObject go = findGameObject();
        //if (go == null)
        //{
        //    SceneManager.LoadScene(sceneName);
        //}
        //go = findGameObject();
        if (go != null)
        {
            loadState(go);
        }
    }
    public virtual void loadState(GameObject go){}

    public virtual void saveState(MemoryMonoBehaviour go) { }

    public bool isFor(MemoryMonoBehaviour mmb)
    {
        bool nameMatch = objectName == mmb.gameObject.name;
        bool sceneMatch = sceneName == mmb.gameObject.scene.name;
        return nameMatch && sceneMatch;
    }

    public GameObject findGameObject()
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.IsValid())
        {
            foreach (GameObject sceneGo in scene.GetRootGameObjects())
            {
                if (sceneGo.name.Equals(objectName))
                {
                    return sceneGo;
                }
                foreach (Transform childTransform in sceneGo.GetComponentsInChildren<Transform>())
                {
                    GameObject childGo = childTransform.gameObject;
                    if (childGo.name.Equals(objectName))
                    {
                        return childGo;
                    }
                }
            }
        }
        return null;
    }
}
