using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class Laser : NetworkBehaviour {
	[SyncVar (hook="OnDirChange")]
	private Vector2 laserDir;

	[SyncVar (hook="OnLaserToggle")]
	public bool laserOn;
	
	[SyncVar (hook="OnColorChange")]
	public PaletteColorID colorID = PaletteColorID.BLACK;

	private LayerMask layersToHit;

	// Use this for initialization
	void Start () {
		Debug.Log ("ID: " + this.netId);
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
		UpdateLaser ();
		CmdSetLaserDir (laserDir);
	}

	[Command]
	void CmdSetLaserDir(Vector2 laserDir) {
		this.laserDir = laserDir;
	}

	void UpdateLaser() {
		laserDir.Normalize ();

		Debug.Log ("Here");

		RaycastHit2D raycastHit = Physics2D.Raycast (transform.position, laserDir, Mathf.Infinity, layersToHit);

		float rotZ = Mathf.Atan2 (laserDir.y, laserDir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0f, 0f, rotZ);
	}

	void OnDirChange(Vector2 laserDir) {
		if (!hasAuthority) {
			UpdateLaser ();
		}
	}

	void OnLaserToggle(bool laserOn) {

	}

	void OnColorChange(PaletteColorID colorID) {
		this.GetComponent<SpriteRenderer>().color = new PaletteColor(colorID).ToColor();
	}
}
