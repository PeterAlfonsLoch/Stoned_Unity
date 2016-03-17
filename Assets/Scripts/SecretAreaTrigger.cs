using UnityEngine;
using System.Collections;

public class SecretAreaTrigger : MonoBehaviour {

    public GameObject secretHider;
    private GameObject playerObject;

	// Use this for initialization
	void Start () {
        playerObject = GameObject.FindGameObjectWithTag("Player");
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.Equals(playerObject))
        {
            secretHider.AddComponent<Fader>();
        }
    }
}
