using UnityEngine;
using System.Collections;

public class CrackedGroundChecker : MonoBehaviour {

    public float forceThreshold = 10;//how much force is required to break it
    public AudioClip breakSound;
    public GameObject secretHider;//the hidden area to be shown when this cracked ground breaks

	// Use this for initialization
	void Start () {
	
	}
	
	//// Update is called once per frame
	//void Update () {
	
	//}

    void OnCollisionEnter2D(Collision2D coll)
    {
        GameObject other = coll.gameObject;
        Rigidbody2D rb2d = other.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            float force = rb2d.velocity.magnitude * rb2d.mass;
            Debug.Log("force: " + force);
            if (force > forceThreshold)
            {
                crackAndBreak();
            }
        }
    }

    private void crackAndBreak()
    {
        AudioSource.PlayClipAtPoint(breakSound, transform.position);
        if (secretHider != null)
        {
            secretHider.AddComponent<Fader>();
        }
        Destroy(gameObject);
    }
}
