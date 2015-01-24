using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	private Vector3 cameraTarget;
	public float speed = .5f;

	public float x_offset;
	public float y_offset;

	public bool lock_y;
	public GameObject player;

	// Use this for initialization
	void Start () {
		cameraTarget = target.position + Vector3.up * 3 - Vector3.forward * 15;
		transform.position = cameraTarget;
		//player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

		
	
	}
	
	// Update is called once per frame
	void Update () {
		cameraTarget = target.position + Vector3.up * y_offset +Vector3.right * x_offset - Vector3.forward * 15;
		if (lock_y) {
			cameraTarget.y = 0;		
		}

		transform.position = (Vector3.Lerp(transform.position, cameraTarget, speed));

		
	
	}
}
