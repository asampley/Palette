using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
	[SyncVar] public int colorID;

	// Use this for initialization
	void Start () {
		this.GetComponent<SpriteRenderer> ().color = PaletteColor.FromInt(colorID).ToColor();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
