using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class Laser : NetworkBehaviour {
	[SyncVar (hook="OnDirChange")]
	private Vector2 laserDir;

	[SyncVar (hook="OnStartChange")]
	private Vector2 laserStart;

	[SyncVar (hook="OnLaserToggle")]
	private bool laserOn = true;
	
	[SyncVar (hook="OnColorChange")]
	private PaletteColorID colorID = PaletteColorID.WHITE;

	private LayerMask layersToHit;

	// Use this for initialization
	void Start () {
		OnColorChange (colorID);
		OnLaserToggle (laserOn);
		OnDirChange (laserDir);
		SetLaserOn (laserOn);
	}

	void InitLayers() {
		foreach (PaletteColorID id in Enum.GetValues(typeof(PaletteColorID))) {
			if (id == PaletteColorID.BLACK) {
				continue;
			};

			layersToHit |= LayerMask.GetMask(new PaletteColor(id).ToString());
		}

		Debug.Log (layersToHit.ToString());
	}

	public void SetLaserDir(Vector2 laserDir) {
		this.laserDir = laserDir;
		UpdateLaserDir ();
		CmdSetLaserDir (laserDir);
	}

	public void SetLaserStart(Vector2 laserStart) {
		this.laserStart = laserStart;
		UpdateLaserDir ();
		CmdSetLaserStart (laserStart);
	}

	public void SetLaserOn(bool isOn) {
		this.laserOn = isOn;
		UpdateLaserOn ();
		CmdSetLaserOn (isOn);
	}

	[Command]
	void CmdSetLaserDir(Vector2 laserDir) {
		this.laserDir = laserDir;
	}

	[Command]
	void CmdSetLaserStart(Vector2 laserStart) {
		this.laserStart = laserStart;
	}

	[Command]
	void CmdSetLaserOn(bool laserOn) {
		this.laserOn = laserOn;
	}

	void OnDirChange(Vector2 laserDir) {
		if (!hasAuthority) {
			this.laserDir = laserDir;
			UpdateLaserDir ();
		}
	}

	void OnStartChange(Vector2 laserStart) {
		if (!hasAuthority) {
			this.laserStart = laserStart;
			UpdateLaserDir ();
		}
	}

	void OnLaserToggle(bool laserOn) {
		if (!hasAuthority) {
			this.laserOn = laserOn;
			UpdateLaserOn ();
		}
	}

	void OnColorChange(PaletteColorID colorID) {
		Debug.Log ("Set laser color to " + new PaletteColor(colorID));
		this.GetComponent<SpriteRenderer>().color = new PaletteColor(colorID).ToColor();
	}

	void UpdateLaserDir() {
		laserDir.Normalize ();

		RaycastHit2D raycastHit = Physics2D.Raycast (transform.position, laserDir, Mathf.Infinity, layersToHit);

		float rotZ = Mathf.Atan2 (laserDir.y, laserDir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0f, 0f, rotZ);
		transform.position = laserStart;
	}

	void UpdateLaserOn() {
		Debug.Log ("Turned " + (laserOn ? "on" : "off") + " laser " + netId);

		this.GetComponent<SpriteRenderer> ().enabled = laserOn;
	}

	public void Toggle() {
		this.SetLaserOn (!laserOn);
	}
}
