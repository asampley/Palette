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
		PaletteColor color = PaletteColor.FromInt (colorID);

		this.GetComponent<SpriteRenderer> ().color = color.ToColor ();
		Debug.Log("Set player " + this + " to have color " + color);

		this.gameObject.layer = color.ToLayer ();
		Debug.Log("Set player " + this + " to be in layer " + color.ToLayer() + "(" + LayerMask.LayerToName(color.ToLayer()) + ")");
	}
}
