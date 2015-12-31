using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;
    private Camera cam;

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
        cam = GetComponent<Camera>();

    }

    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (cam.orthographicSize < player.GetComponent<PlayerController>().baseRange*2)
            {
                cam.orthographicSize += 1f;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (cam.orthographicSize > 1)
            {
                cam.orthographicSize -= 1f;
            }
        }
    }
	
	// Update is called once per frame, after all other objects have moved that frame
	void LateUpdate ()
    {
        //transform.position = player.transform.position + offset;
        transform.position = Vector3.MoveTowards(
            transform.position, 
            player.transform.position + offset, 
            Vector3.Distance(
                transform.position, 
                player.transform.position) * 2
                * Time.deltaTime);
	}
}
