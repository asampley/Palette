using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpinner : MonoBehaviour {
	public Rigidbody2D rb2d;
	public float degPerSec;

	// Update is called once per frame
	void Update () {
		rb2d.rotation += degPerSec * Time.deltaTime;
	}
}
