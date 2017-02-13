using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
	[SyncVar (hook="OnColorChange")]
	public PaletteColorID colorID;
	public GameObject head;

	private static LayerMask groundLayerMask;

	static void InitGroundLayerMask() {
		groundLayerMask = 0x0;

		foreach (PaletteColorID id in Enum.GetValues(typeof(PaletteColorID))) {
			groundLayerMask |= LayerMask.GetMask(new PaletteColor (id).ToLayerName ());
		}
	}

	// Use this for initialization
	void Start () {
		InitGroundLayerMask ();
		OnColorChange (colorID);
	}

	public LayerMask GroundLayerMask() {
		if (groundLayerMask == null) {
			Player.InitGroundLayerMask ();
		}
		return groundLayerMask ^ LayerMask.GetMask(new PaletteColor(this.colorID).ToLayerName());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer ();

		GameObject.Find ("Main Camera").GetComponent<CameraFollow> ().player = this.gameObject;
		SceneData.sceneObject.GetComponent<LocalPlayer> ().localPlayer = this.gameObject;
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
			SceneData.sceneObject.GetComponent<PlayerColor> ().SetLocalPlayerColor(colorID);
		}
	}
}
