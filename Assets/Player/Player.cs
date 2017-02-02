using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
	[SyncVar (hook="OnColorChange")]
	public PaletteColorID colorID;
	public GameObject head;

	// Use this for initialization
	void Start () {
		OnColorChange (colorID);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer ();

		GameObject.Find ("Main Camera").GetComponent<CameraFollow> ().player = this.gameObject;
	}

	public override void OnStartClient ()
	{
		base.OnStartClient ();

		// TODO: add all players to the main camera for more fancy movement.
	}

	/**
	 * Called when the variable colorID is changed.
	 */
	void OnColorChange(PaletteColorID colorID) {
		PaletteColor color = new PaletteColor(colorID);
		int layer = color.ToEntityLayer ();

		this.GetComponent<SpriteRenderer> ().color = color.ToColor ();
		//Debug.Log("Set player " + this + " to have color " + color);
		head.GetComponent<SpriteRenderer> ().color=color.ToColor ();

		this.gameObject.layer = layer;
		//Debug.Log("Set player " + this + " to be in layer " + layer + "(" + LayerMask.LayerToName(layer) + ")");

		if (isLocalPlayer) {
			GameObject.Find ("Client Data").GetComponent<PlayerColor> ().SetPlayerColor(colorID);
		}
	}
}
