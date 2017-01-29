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

	private LayerMask layersColors;
	private LayerMask layersToHit;

	private ColorAdder affectedObject;

	// Use this for initialization
	void Start () {
		InitLayers ();
		SetLaserColor (colorID);
		SetLaserOn (laserOn);
		SetLaserDir (laserDir);
		SetLaserOn (laserOn);
	}

	void InitLayers() {
		layersColors = 0;

		foreach (PaletteColorID id in Enum.GetValues(typeof(PaletteColorID))) {
			layersColors |= LayerMask.GetMask(new PaletteColor(id).ToLayerName());
		}
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

	public void SetLaserColor(PaletteColorID colorID) {
		UpdateLaserColor (colorID);
		CmdSetLaserColor (colorID);
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

	[Command]
	void CmdSetLaserColor(PaletteColorID colorID) {
		this.colorID = colorID;
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
		if (!hasAuthority) {
			UpdateLaserColor (colorID);
		}
	}

	void UpdateLaserDir() {
		laserDir.Normalize ();

		// Handle colisions and color
		RaycastHit2D raycastHit = Physics2D.Raycast (laserStart, laserDir, Mathf.Infinity, layersToHit);
		try {
			ColorAdder colorAdder = raycastHit.collider.GetComponent<ColorAdder>();
			if (affectedObject == null || affectedObject != colorAdder) {
				colorAdder.AddColor (new PaletteColor(colorID));
			}

			//Debug.Log ("Laser hit " + raycastHit.collider.gameObject.name);

			if (this.affectedObject != colorAdder && this.affectedObject != null) {
				this.affectedObject.RemoveColor(new PaletteColor(colorID));
				//Debug.Log("Laser stopped hitting " + this.affectedObject);
			}

			this.affectedObject = colorAdder;
		} catch (NullReferenceException e) {
			//Debug.Log("Laser stopped hitting " + this.affectedObject);
			if (this.affectedObject != null) {
				this.affectedObject.RemoveColor (new PaletteColor (colorID));
			}
			this.affectedObject = null;
		}


		// handle drawing of sprite
		float rotZ = Mathf.Atan2 (laserDir.y, laserDir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0f, 0f, rotZ); // set rotation

		float length = 1 * raycastHit.distance;
		Debug.Log (raycastHit.distance);
		if (length == 0) {
			length = 1000;
		}

		Vector3 newScale = transform.localScale;
		newScale.x = length; // IMPORTANT: assumes sprite unit size of 1 in x coords
		transform.localScale = newScale; // set length

		transform.position = laserStart + length / 2 * laserDir;
	}

	void UpdateLaserOn() {
		Debug.Log ("Turned " + (laserOn ? "on" : "off") + " laser " + netId);

		this.GetComponent<SpriteRenderer> ().enabled = laserOn;
	}

	void UpdateLaserColor(PaletteColorID newColorID) {
		if (this.affectedObject != null) {
			this.affectedObject.RemoveColor (new PaletteColor (colorID));
			//Debug.Log("Laser stopped hitting " + this.affectedObject);
		}

		this.colorID = newColorID;

		Debug.Log ("Set laser color to " + new PaletteColor(colorID));
		this.GetComponent<SpriteRenderer>().color = new PaletteColor(colorID).ToColor();
		layersToHit = layersColors ^ LayerMask.GetMask (new PaletteColor (colorID).ToLayerName ());
		//Debug.Log ("Laser raycast set to hit " + string.Format("{0:X}", (int)layersToHit));
	}

	public void Toggle() {
		this.SetLaserOn (!laserOn);
	}
}
