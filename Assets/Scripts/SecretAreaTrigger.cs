using UnityEngine;
using System.Collections;

public class SecretAreaTrigger : MemoryMonoBehaviour {

    public GameObject secretHider;
    private GameObject playerObject;

    public bool fading = false;

	// Use this for initialization
	void Start () {
        playerObject = GameObject.FindGameObjectWithTag("Player");
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.Equals(playerObject))
        {
            fade();
        }
    }

    void fade()//for use by this class only (mainly for the mechanical inner workings of it)
    {
        fading = true;
        secretHider.AddComponent<Fader>();
        GameManager.saveMemory(this);
    }
    public void destroy(){ //for use by outside functions (particular the memory object for this class)
        fading = true;
        GameManager.saveMemory(this);
        Destroy(secretHider);
    }

    public override MemoryObject getMemoryObject()
    {
        return new SecretAreaTriggerMemory(this);
    }
}

//
//Class that saves the important variables of this class
//
public class SecretAreaTriggerMemory : MemoryObject
{
    public SecretAreaTriggerMemory() { }//only called by the method that reads it from the file
    public SecretAreaTriggerMemory(SecretAreaTrigger sat): base(sat)
    {
        saveState(sat);
    }

    public override void loadState(GameObject go)
    {
        SecretAreaTrigger sat = go.GetComponent<SecretAreaTrigger>();
        if (sat != null)
        {
            if (this.found)
            {
                sat.destroy();
            }
        }
    }
    public override void saveState(MemoryMonoBehaviour mmb)
    {
        SecretAreaTrigger sat = ((SecretAreaTrigger)mmb);
        this.found = sat.fading;
    }
}
