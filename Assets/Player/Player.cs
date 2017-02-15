using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
	[SyncVar (hook="OnColorChange")]
	public PaletteColorID colorID;
	public GameObject head;

	[SyncVar (hook="OnNumberChange")]
	private int number = -1;

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

		// disable until a number is picked
		if (number == -1) {
			//this.gameObject.SetActive (false);
			Activate (false);
		}
	}

	public LayerMask GroundLayerMask() {
		if (groundLayerMask == null) {
			Player.InitGroundLayerMask ();
		}
		return groundLayerMask ^ LayerMask.GetMask(new PaletteColor(this.colorID).ToLayerName());
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

	public void Activate(bool active) {
		foreach (SpriteRenderer sr in GetComponents<SpriteRenderer>()) {
			sr.enabled = active;
		}
		foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()) {
			sr.enabled = active;
		}
		PlayerController control = GetComponent<PlayerController> ();
		control.enabled = active;
	}

	public int GetNumber() {
		return this.number;
	}

	public void SetNumber(int number) {
		if (hasAuthority 
		    && !(SceneData.sceneObject.GetComponent<PlayerSpawn>().GetPlayerSpawned(number))) {
			Debug.Log ("Spawning player " + number);
			CmdSetNumber (number);
			UpdateNumber (number);
		}
	}

	[Command]
	void CmdSetNumber(int number) {
		this.number = number;
		this.colorID = SceneData.sceneObject.GetComponent<PlayerSpawn> ().GetPlayerColorID (number);
	}

	void OnNumberChange(int number) {
		if (!hasAuthority) {
			UpdateNumber (number);
		}
	}


	/**
	 * Called when the variable colorID is changed.
	 */
	void OnColorChange(PaletteColorID colorID) {
		UpdateColor (colorID);
	}

	void UpdateNumber(int number) {
		if (number == -1) {
			//this.gameObject.SetActive(false);
			Activate (false);
		} else {
			this.gameObject.SetActive(true);
			Activate (true);
			PlayerSpawn info = SceneData.sceneObject.GetComponent<PlayerSpawn> ();
			info.SetPlayerSpawned (number, false);
			if (hasAuthority) {
				this.transform.position = info.GetPlayerSpawn (number).position;
			}
			info.SetPlayerSpawned (number, true);
		}

		Debug.Log ("Changed " + this.name + " to number " + number);

		this.number = number;
	}

	void UpdateColor(PaletteColorID colorID) {
		this.colorID = colorID;

		PaletteColor color = new PaletteColor(colorID);
		int layer = color.ToEntityLayer ();

		this.GetComponent<SpriteRenderer> ().color = color.ToColor ();
		Debug.Log("Set player " + this + " to have color " + color);
		head.GetComponent<SpriteRenderer> ().color = color.ToColor ();

		this.gameObject.layer = layer;
		//Debug.Log("Set player " + this + " to be in layer " + layer + "(" + LayerMask.LayerToName(layer) + ")");

		if (isLocalPlayer) {
			SceneData.sceneObject.GetComponent<PlayerColor> ().SetLocalPlayerColor(colorID);
		}

		this.GetComponent<LaserController> ().UpdateLaserColor ();

	}
}
