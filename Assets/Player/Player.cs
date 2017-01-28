using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
	[SyncVar (hook="OnColorChange")]
	public PaletteColorID colorID;

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
	void OnColorChange(PaletteColorID colorID) {
		PaletteColor color = new PaletteColor(colorID);
		int layer = color.ToEntityLayer ();

		this.GetComponent<SpriteRenderer> ().color = color.ToColor ();
		Debug.Log("Set player " + this + " to have color " + color);

		this.gameObject.layer = layer;
		//Debug.Log("Set player " + this + " to be in layer " + layer + "(" + LayerMask.LayerToName(layer) + ")");
	}
}
