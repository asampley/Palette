using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;


public class CameraFollow : MonoBehaviour {

	private Vector2 velocity;

	//floats
	public float smoothTimeY = 0.05f;
	public float smoothTimeX = 0.05f;
    public float minSizeY = 10f;
    public float padding = 2f;

    private GameObject player;

	//if we want to do two person camera setup.
	public bool dynamicCam = true;
	// if we want bounds
	public bool bounds;

	public Vector3 minCameraPos;
	public Vector3 maxCameraPos;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update() {
		if (dynamicCam && SceneData.sceneObject) {
			Dictionary<int, Player> players = SceneData.sceneObject.GetComponent<LocalPlayer> ().GetPlayers ();
	
			if (players.Count != 0) { 
			
				Vector3 middle = Vector3.zero;
				int count = 0;

				float x_sub = 0;
				float y_sub = 0;
				bool first = true;
				foreach (Player player in players.Values) {
					middle += player.transform.position;
					if (first) {
						x_sub += player.transform.position.x;
						y_sub += player.transform.position.y;
						first = false;
					} else {
						x_sub -= player.transform.position.x;
						y_sub -= player.transform.position.y;
					}
					count += 1;
				}
				middle /= count;
				middle.z = transform.position.z;
				transform.position = middle;

				// from http://answers.unity3d.com/questions/674225/2d-camera-to-follow-two-players.html
				//horizontal size is based on actual screen ratio
				float minSizeX = minSizeY * Screen.width / Screen.height;
				//multiplying by 0.5, because the ortographicSize is actually half the height
				float width = Mathf.Abs (x_sub) * 0.5f + padding;
				float height = Mathf.Abs (y_sub) * 0.5f + padding;
				//computing the size
				float camSizeX = Mathf.Max (width, minSizeX);
				gameObject.GetComponent<Camera> ().orthographicSize = Mathf.Max (height,
				                                                                 camSizeX * Screen.height / Screen.width, minSizeY);
            
			}
		} else if (SceneData.sceneObject) { // do a one person camera	
			///* Smoothing code below from old camera
			player = SceneData.sceneObject.GetComponent<LocalPlayer> ().localPlayer.gameObject;
			float posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
			float posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);
			//again z needs to be taken account of as it is 2D, just make it not do anything
			transform.position = new Vector3 (posX, posY, transform.position.z);
			if (bounds) {
				transform.position = new Vector3 (Mathf.Clamp (transform.position.x, minCameraPos.x, maxCameraPos.x),
				                                  Mathf.Clamp (transform.position.y, minCameraPos.y, maxCameraPos.y),
				                                  Mathf.Clamp (transform.position.z, minCameraPos.z, maxCameraPos.z));
			}
			//*/
		} else {

		}
    }

}