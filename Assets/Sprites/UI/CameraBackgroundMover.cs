using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class CameraBackgroundMover : MonoBehaviour {

    public Vector2 vel;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Rigidbody2D>().velocity = vel;
    }
}
