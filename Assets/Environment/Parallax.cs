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
        
        if (parallaxFactor.Equals(0f))
        {
            // It is the static background that should be centered.
            center.x = -GetComponent<SpriteRenderer>().bounds.size.x / 2;
        } else
        {
            // Parallax image is a repeat of 3 segments, and center should be the 2nd segment.
            center.x = -(GetComponent<SpriteRenderer>().bounds.size.x / 3);
        }
		center.y = GetComponent<SpriteRenderer> ().bounds.size.y / 2;
		localPos.x = center.x - Mod(parallaxFactor * transform.parent.position.x, GetComponent<SpriteRenderer> ().bounds.size.x / 3);
		localPos.y = center.y;
		localPos.z = -transform.parent.position.z + globalZ;// always set to have global position to avoid moving with the camera
		transform.localPosition = localPos;
	}

	float Mod(float a, float b) {
		return a - b * Mathf.Floor (a / b);
	}
}
