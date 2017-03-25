﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class Parallax : MonoBehaviour {
	public float parallaxFactor = 1f;
	public float globalZ = 0;

	void Update() {
		Vector3 localPos = transform.localPosition;
		Vector3 center = Vector3.zero;
		center.x = -GetComponent<SpriteRenderer> ().bounds.size.x / 4;
		center.y = GetComponent<SpriteRenderer> ().bounds.size.y / 2;
		localPos.x = center.x - Mod(parallaxFactor * transform.parent.position.x, GetComponent<SpriteRenderer> ().bounds.size.x / 2);
		localPos.y = center.y;
		localPos.z = -transform.parent.position.z + globalZ;// always set to have global position to avoid moving with the camera
		transform.localPosition = localPos;
	}

	float Mod(float a, float b) {
		return a - b * Mathf.Floor (a / b);
	}
}