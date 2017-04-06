using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
	public Transform spawn { get; set; }
	public PaletteColorID spawnColor { get; set; }

	public GameObject head;

	[SyncVar (hook="OnColorChange")]
	private PaletteColorID colorID;

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
		UpdateNumber (number);
		OnColorChange (colorID);

		// disable until a number is picked
		if (number == -1) {
			Activate (false);
		}
	}

	public void Respawn() {
		PlayerSpawn info = SceneData.sceneObject.GetComponent<PlayerSpawn> ();
		if (isLocalPlayer) {
			this.transform.position = spawn.position;
		}

		this.SetColor(spawnColor);
		info.SetPlayerSpawned (number, true);

		Debug.Log ("Spawned player " + number);
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
		SceneData.sceneObject.GetComponent<LocalPlayer> ().localPlayer = this;
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
		LaserController lControl = GetComponent<LaserController> ();
		control.enabled = active;
		lControl.enabled = active;

//		Cursor.lockState = active ? CursorLockMode.Confined : CursorLockMode.None;
//		Cursor.visible = active;
	}

	public PaletteColorID GetColorID() {
		return colorID;
	}

	public int GetNumber() {
		return this.number;
	}

	public void SetNumber(int number) {
		if (isLocalPlayer
		    && !(SceneData.sceneObject.GetComponent<PlayerSpawn>().GetPlayerSpawned(number))) {
			Debug.Log ("Spawning player " + number);
			CmdSetNumber (number);
			UpdateNumber (number);
			UpdateColor (colorID);
		}
	}

	public void SetColor(PaletteColorID colorID) {
		if (isLocalPlayer) {
			CmdSetColor (colorID);
			UpdateColor (colorID);
		}
	}

	[Command]
	void CmdSetNumber(int number) {
		this.number = number;
		this.SetColor(SceneData.sceneObject.GetComponent<PlayerSpawn> ().GetPlayerColorID (number));
		UpdateNumber (number);
	}

	[Command]
	void CmdSetColor(PaletteColorID colorID) {
		this.colorID = colorID;
		UpdateColor (colorID);
	}

	void OnNumberChange(int number) {
		if (!isLocalPlayer) {
			UpdateNumber (number);
		}
	}

	/**
	 * Called when the variable colorID is changed.
	 */
	void OnColorChange(PaletteColorID colorID) {
		if (!isLocalPlayer) {
			UpdateColor (colorID);
		}
	}

	void UpdateNumber(int number) {
		PlayerSpawn info = SceneData.sceneObject.GetComponent<PlayerSpawn> ();
		LocalPlayer lp = SceneData.sceneObject.GetComponent<LocalPlayer> ();

		lp.SetPlayer (number, null);

		Debug.Log ("Changed " + this.name + " to number " + number);
		this.number = number;

		if (number == -1) {
			Activate (false);
		} else {
			lp.SetPlayer (this.number, this);

			info.SetPlayerSpawned (this.number, false);
			this.gameObject.SetActive(true);
			Activate (true);
			this.GetComponent<Animator> ().runtimeAnimatorController = info.GetPlayerAnimatorController (number);
			this.spawn = info.GetPlayerSpawn (number);
			this.spawnColor = info.GetPlayerColorID (number);
			if (isLocalPlayer) {
				head.transform.localPosition = info.GetPlayerHeadPosition (number);
			}
			head.GetComponent<SpriteRenderer> ().sprite = info.GetPlayerHeadSprite (number);
//			_head = null;
			Respawn ();
		}
	}

	void UpdateColor(PaletteColorID colorID) {
		this.colorID = colorID;

		PaletteColor color = new PaletteColor(colorID);
		int layer = color.ToEntityLayer ();

		this.GetComponent<SpriteRenderer> ().color = color.ToColor ();
		Debug.Log("Set player " + this + " to have color " + color);
//		try {
//			head.GetComponent<SpriteRenderer> ().color = color.ToColor ();
//		} catch (NullReferenceException) {
//
//		}
		head.GetComponent<SpriteRenderer> ().color = new PaletteColor(colorID).ToColor();

		this.gameObject.layer = layer;
		//Debug.Log("Set player " + this + " to be in layer " + layer + "(" + LayerMask.LayerToName(layer) + ")");

		if (isLocalPlayer) {
			SceneData.sceneObject.GetComponent<PlayerColor> ().SetLocalPlayerColor(colorID);
		}

		this.GetComponent<LaserController> ().UpdateLaserColor ();

	}
}
