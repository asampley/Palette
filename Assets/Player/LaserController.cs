using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LaserController : NetworkBehaviour {
    public int rotationOffset = 0; // degrees
    // Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		rotate();
	}

	void rotate() {
		Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;	// subtract pos of player from mouse pos
		difference.Normalize(); // Normalize the vector. this means that all the sum of vector will be equal to 1.
		float rotZ = Mathf.Atan2(difference.y,difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f,0f,rotZ + rotationOffset);
	}

}