﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	[SerializeField] private float xVel = 0.05f;
	[SerializeField] private float yVel = 0.05f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) return;

		float dx = Input.GetAxis("Horizontal") * xVel;
		float dy = Input.GetAxis("Vertical") * yVel;

		transform.Translate(dx, dy, 0);
	}
}
