using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class Laser : NetworkBehaviour {
	[SyncVar (hook="OnDirChange")]
	private Vector2 laserDir;

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

	[Command]
	void CmdSetLaserDir(Vector2 laserDir) {
		this.laserDir = laserDir;
	}

	void OnDirChange(Vector2 laserDir) {
		if (!hasAuthority) {
			UpdateLaserDir ();
		}
	}

	void UpdateLaserDir() {
		laserDir.Normalize ();

		Debug.Log ("Here");

		RaycastHit2D raycastHit = Physics2D.Raycast (transform.position, laserDir, Mathf.Infinity, layersToHit);

		float rotZ = Mathf.Atan2 (laserDir.y, laserDir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0f, 0f, rotZ);
	}

	public void SetLaserOn(bool isOn) {
		this.laserOn = isOn;
		CmdSetLaserOn (isOn);
	}

	[Command]
	void CmdSetLaserOn(bool laserDir) {
		this.laserOn = laserOn;
	}

	void OnLaserToggle(bool laserOn) {
		if (!hasAuthority) {
			UpdateLaserOn ();
		}
	}

	void UpdateLaserOn() {
		this.GetComponent<SpriteRenderer> ().enabled = laserOn;
	}

	void OnColorChange(PaletteColorID colorID) {
		Debug.Log ("Set laser color to " + new PaletteColor(colorID));
		this.GetComponent<SpriteRenderer>().color = new PaletteColor(colorID).ToColor();
	}
}
