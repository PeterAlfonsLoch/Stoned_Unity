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
        srs.Add(GetComponent<Ferr2DT_PathTerrain>());
        srs.AddRange(GetComponentsInChildren<SpriteRenderer>());
        srs.AddRange(GetComponentsInChildren<Ferr2DT_PathTerrain>());
        foreach (Collider2D bc in GetComponentsInChildren<Collider2D>())
        {
            Destroy(bc);
            //bc.enabled = false;
        }
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.sortingOrder = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime <= Time.time)
        {
            foreach (Object o in srs)
            {
                if (o is SpriteRenderer)
                {
                    SpriteRenderer sr = (SpriteRenderer)o;
                    Color prevColor = sr.color;
                    float t = (Time.time - startTime) / duration;//2016-03-17: copied from an answer by treasgu (http://answers.unity3d.com/questions/654836/unity2d-sprite-fade-in-and-out.html)
                    sr.color = new Color(prevColor.r, prevColor.g, prevColor.b, Mathf.SmoothStep(startfade, endfade, t));
                    if (sr.color.a == endfade)
                    {
                        Destroy(gameObject);
                    }
                }
                if (o is Ferr2DT_PathTerrain)
                {
                    Ferr2DT_PathTerrain sr = (Ferr2DT_PathTerrain)o;
                    Color prevColor = sr.vertexColor;
                    float t = (Time.time - startTime) / duration;//2016-03-17: copied from an answer by treasgu (http://answers.unity3d.com/questions/654836/unity2d-sprite-fade-in-and-out.html)
                    sr.vertexColor = new Color(prevColor.r, prevColor.g, prevColor.b, Mathf.SmoothStep(startfade, endfade, t));
                    if (sr.vertexColor.a == endfade)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
