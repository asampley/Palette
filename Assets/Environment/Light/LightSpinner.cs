using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpinner : MonoBehaviour {
	public Rigidbody2D rb2d;

	// Update is called once per frame
	void Update () {
		rb2d.rotation += 30 * Time.deltaTime;
	}
}
