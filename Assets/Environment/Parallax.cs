using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Parallax : MonoBehaviour {
	public float parallaxFactor = 1f;

	void Update() {
		Vector3 localPos = transform.localPosition;
		localPos.x = (parallaxFactor * transform.position.x) % GetComponent<SpriteRenderer> ().bounds.size.x;
	}
}
