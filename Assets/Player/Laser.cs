using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public enum LaserMode { ADD, SUBTRACT }

public class Laser : NetworkBehaviour {
	[SyncVar (hook="OnDirChange")]
	private Vector2 laserDir;

	[SyncVar (hook="OnStartChange")]
	private Vector2 laserStart;

	[SyncVar (hook="OnLaserToggle")]
	private bool laserOn = true;

	[SyncVar (hook="OnLaserMode")]
	private LaserMode mode;

	[SyncVar (hook="OnColorChange")]
	private PaletteColorID colorID = PaletteColorID.WHITE;

	private LayerMask layersColors;
	private LayerMask layersToHit;

	private ColorAdder affectedObject;

	public Sprite laserAdd;
	public Sprite laserSub;

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

		layersToHit = layersColors ^ LayerMask.GetMask (new PaletteColor (colorID).ToLayerName ());
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

	public void SetLaserMode(LaserMode mode) {
		UpdateLaserMode (mode);
		CmdSetLaserMode (mode);
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

	[Command]
	void CmdSetLaserMode(LaserMode mode) {
		this.mode = mode;
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

	void OnLaserMode(LaserMode mode) {
		if (!hasAuthority) {
			UpdateLaserMode (mode);
		}
	}

	void UpdateLaserDir() {
		laserDir.Normalize ();

		// Handle colisions and color
		if (laserOn) {
			RaycastHit2D raycastHit = Physics2D.Raycast (laserStart, laserDir, Mathf.Infinity, layersToHit);

			ColorAdder newAffectedObject = null;
			try {
				newAffectedObject = raycastHit.collider.GetComponent<ColorAdder> ();
			} catch (NullReferenceException e) {
				// It's fine, the new affected object is just null
			}

			if (this.affectedObject == newAffectedObject) { // if new and old are the same, do nothing

			} else {
				if (this.affectedObject != null) { // if they are different, and old is an object, subtract
					switch (mode) {
					case LaserMode.ADD:
						this.affectedObject.RemoveAdditiveColor (new PaletteColor (colorID));
						break;
					case LaserMode.SUBTRACT:
						this.affectedObject.RemoveSubtractiveColor (new PaletteColor (colorID));
						break;
					}
				}

				if (newAffectedObject != null) { // if they are different, and the new is an object, add
					switch (mode) {
					case LaserMode.ADD:
						newAffectedObject.AddAdditiveColor (new PaletteColor (colorID));
						break;
					case LaserMode.SUBTRACT:
						newAffectedObject.AddSubtractiveColor (new PaletteColor (colorID));
						break;
					}
				}
			}

			// update affected object
			this.affectedObject = newAffectedObject;

			// handle drawing of sprite
			float rotZ = Mathf.Atan2 (laserDir.y, laserDir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (0f, 0f, rotZ); // set rotation

			float length = 1 * raycastHit.distance;
			//Debug.Log (raycastHit.distance);
			if (length == 0) {
				length = 1000;
			}

			Vector3 newScale = transform.localScale;
			newScale.x = length; // IMPORTANT: assumes sprite unit size of 1 in x coords
			transform.localScale = newScale; // set length

			transform.position = laserStart + length / 2 * laserDir;
		} else if (this.affectedObject != null) { // remove self from object when off
			switch (mode) {
				case LaserMode.ADD:
				this.affectedObject.RemoveAdditiveColor (new PaletteColor (colorID));
				break;
				case LaserMode.SUBTRACT:
				this.affectedObject.RemoveSubtractiveColor (new PaletteColor (colorID));
				break;
			}

			this.affectedObject = null;
		}
	}

	void UpdateLaserOn() {
		Debug.Log ("Turned " + (laserOn ? "on" : "off") + " laser " + netId);

		this.GetComponent<SpriteRenderer> ().enabled = laserOn;

		// Refresh laser calculations
		UpdateLaserDir ();
	}

	void UpdateLaserColor(PaletteColorID newColorID) {
		if (this.affectedObject != null) {
			switch (this.mode) {
				case LaserMode.ADD:
					this.affectedObject.RemoveAdditiveColor (new PaletteColor (colorID));
				break;
				case LaserMode.SUBTRACT:
					this.affectedObject.RemoveSubtractiveColor (new PaletteColor (colorID));
				break;
			}
		}

		this.colorID = newColorID;

		Debug.Log ("Set laser color to " + new PaletteColor(colorID));
		this.GetComponent<SpriteRenderer>().color = new PaletteColor(colorID).ToColor();

		// Do not hit own color
		layersToHit = layersColors ^ LayerMask.GetMask (new PaletteColor (colorID).ToLayerName ());
		//Debug.Log ("Laser raycast set to hit " + string.Format("{0:X}", (int)layersToHit));
	}

	void UpdateLaserMode(LaserMode mode) {
		if (mode == this.mode) return;
		
		if (this.affectedObject != null) {
			switch (this.mode) {
			case LaserMode.ADD:
				this.affectedObject.RemoveAdditiveColor (new PaletteColor (colorID));
				this.affectedObject.AddSubtractiveColor (new PaletteColor (colorID));
				break;
			case LaserMode.SUBTRACT:
				this.affectedObject.RemoveSubtractiveColor (new PaletteColor (colorID));
				this.affectedObject.AddAdditiveColor (new PaletteColor (colorID));
				break;
			}
		}

		this.mode = mode;

		switch (mode) {
		case LaserMode.ADD:
			this.GetComponent<SpriteRenderer> ().sprite = laserAdd;
			break;
		case LaserMode.SUBTRACT:
			this.GetComponent<SpriteRenderer> ().sprite = laserSub;
			break;
		}

	}

	public void ToggleOn() {
		this.SetLaserOn (!laserOn);
	}

	public void ToggleMode() {
		switch (mode) {
		case LaserMode.ADD:
			this.SetLaserMode (LaserMode.SUBTRACT);
			break;
		case LaserMode.SUBTRACT:
			this.SetLaserMode (LaserMode.ADD);
			break;
		}
	}
}
