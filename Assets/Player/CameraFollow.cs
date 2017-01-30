using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class CameraFollow : MonoBehaviour {

	private Vector2 velocity;

	//floats
	public float smoothTimeY = 0.05f;
	public float smoothTimeX = 0.05f;
	
	public GameObject player;

	//if you wanna make boundaries
	public bool bounds;

	public Vector3 minCameraPos;
	public Vector3 maxCameraPos;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate() {
		if (player != null) {
			float posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
			float posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);
			//again z needs to be taken account of as it is 2D, just make it not do anything
			transform.position = new Vector3 (posX, posY, transform.position.z);
			if (bounds) {
				transform.position = new Vector3 (Mathf.Clamp (transform.position.x, minCameraPos.x, maxCameraPos.x),
				                                Mathf.Clamp (transform.position.y, minCameraPos.y, maxCameraPos.y),
				                                Mathf.Clamp (transform.position.z, minCameraPos.z, maxCameraPos.z));
			}
		}
	}

}
