using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

    public float startfade = 1.0f;
    public float endfade = 0.0f;
    public float fadetime = 30;
    public float delayTime = 0f;
    
    private ArrayList srs;
    private float startTime;
    private float duration;

    // Use this for initialization
    void Start ()
    {
        startTime = Time.time + delayTime;
        duration = Mathf.Abs(startfade - endfade);
        srs = new ArrayList();
        srs.Add(GetComponent<SpriteRenderer>());
        srs.AddRange(GetComponentsInChildren<SpriteRenderer>());
        foreach (BoxCollider2D bc in GetComponentsInChildren<BoxCollider2D>())
        {
            Destroy(bc);
            //bc.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime <= Time.time)
        {
            foreach (SpriteRenderer sr in srs)
            {
                Color prevColor = sr.color;
                float t = (Time.time - startTime) / duration;//2016-03-17: copied from an answer by treasgu (http://answers.unity3d.com/questions/654836/unity2d-sprite-fade-in-and-out.html)
                sr.color = new Color(prevColor.r, prevColor.g, prevColor.b, Mathf.SmoothStep(startfade, endfade, t));
                if (sr.color.a == endfade)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
