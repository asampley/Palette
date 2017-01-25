using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
	[SyncVar (hook="OnColorChange")]
	public int colorID;

	// Use this for initialization
	void Start () {
		OnColorChange (colorID);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**
	 * Called when the variable colorID is changed.
	 */
	void OnColorChange(int colorID) {
		this.GetComponent<SpriteRenderer> ().color = PaletteColor.FromInt (colorID).ToColor ();
		Debug.Log("Changed player " + this + " to have color " + PaletteColor.FromInt(colorID));
	}
}
