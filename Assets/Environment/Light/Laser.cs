using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public enum LaserMode { ADD, SUBTRACT }

public class Laser : NetworkBehaviour {
	public Rigidbody2D rb2d;
	public LaserAnimator anim;

	[SyncVar (hook="OnLaserToggle")]
	private bool laserOn = true;

	public LaserMode initMode;
	[SyncVar (hook="OnLaserMode")]
	private LaserMode mode;

	public PaletteColorID initColor;
	[SyncVar (hook="OnColorChange")]
	private PaletteColorID colorID;

	private LayerMask layersColors;
	private LayerMask layersToHit;

	private ColorAdder affectedObject;

	public Sprite laserAdd;
	public Sprite laserSub;

	public override void OnStartServer ()
	{
		SetLaserColor (initColor);
		SetLaserMode (initMode);
	}

	// Use this for initialization
	void Start () {
		InitLayers ();
		UpdateLaserColor (colorID);
		UpdateLaserOn ();
		UpdateLaserMode (mode);
	}

	void Update() {
		UpdateLaserDir ();
	}

	void InitLayers() {
		layersColors = 0;

		foreach (PaletteColorID id in Enum.GetValues(typeof(PaletteColorID))) {
			layersColors |= LayerMask.GetMask(new PaletteColor(id).ToLayerName());
		}
		layersColors ^= LayerMask.GetMask (PaletteColor.BLACK.ToLayerName ());

		layersToHit = layersColors ^ LayerMask.GetMask (new PaletteColor (colorID).ToLayerName ());
	}

	public void SetLaserOn(bool isOn) {
		if (this.laserOn == isOn) return;

		this.laserOn = isOn;
		UpdateLaserOn ();
		CmdSetLaserOn (isOn);
	}

	public void SetLaserColor(PaletteColorID colorID) {
		if (hasAuthority) {
			Debug.Log ("Set " + this + " to color " + new PaletteColor (colorID));

			if (this.colorID == colorID)
				return;

			UpdateLaserColor (colorID);
			CmdSetLaserColor (colorID);
		}
	}

	public void SetLaserMode(LaserMode mode) {
		if (hasAuthority) {
			if (this.mode == mode)
				return;

			UpdateLaserMode (mode);
			CmdSetLaserMode (mode);
		}
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
		Vector2 laserStart = rb2d.position;
		float rotZ = rb2d.rotation;
		Vector2 laserDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * rotZ), Mathf.Sin(Mathf.Deg2Rad * rotZ));

		// Handle colisions and color
		if (laserOn) {
			RaycastHit2D raycastHit = Physics2D.Raycast (laserStart, laserDir, Mathf.Infinity, layersToHit);

			ColorAdder newAffectedObject = null;
			try {
				newAffectedObject = raycastHit.collider.GetComponent<ColorAdder> ();
			} catch (NullReferenceException e) {
				if (raycastHit.collider == null) {
					// It's fine, the new affected object is just null
				} else {
					throw e;
				}
			}

			if (this.affectedObject == newAffectedObject) { // if new and old are the same, do nothing

			} else { // if they are different, and the new is an object, remove from old, add to new
				this.UnaffectObject ();

				if (newAffectedObject != null) { 
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
			rb2d.transform.rotation = Quaternion.Euler (0f, 0f, rotZ); // set rotation to fix any rounding errors (apparently)

			float length = 1 * raycastHit.distance;
			//Debug.Log (raycastHit.distance);
			if (length == 0) {
				length = 500;
			}

//			Vector3 newScale = anim.transform.localScale;
//			newScale.x = length; // IMPORTANT: assumes sprite unit size of 1 in x coords
//			anim.transform.localScale = newScale; // set length
//
//			anim.transform.position = laserStart + length * laserDir / 2;
			anim.SetLength (length);
		} else { // remove self from object when off
			this.UnaffectObject ();
		}
	}

	void UpdateLaserOn() {
		Debug.Log ("Turned " + (laserOn ? "on" : "off") + " laser " + netId);

		this.anim.SetVisible (laserOn);

		// Refresh laser calculations
		UpdateLaserDir ();
	}

	void UpdateLaserColor(PaletteColorID newColorID) {
		this.UnaffectObject ();

		this.colorID = newColorID;

		Debug.Log ("Set laser " + netId + " color to " + new PaletteColor(colorID));
		//r.color = new PaletteColor(colorID).ToColor();
		anim.SetColor (new PaletteColor (colorID));

		// Do not hit own color
		layersToHit = layersColors ^ LayerMask.GetMask (new PaletteColor (colorID).ToLayerName ());
		//Debug.Log ("Laser raycast set to hit " + string.Format("{0:X}", (int)layersToHit));
	}

	void UpdateLaserMode(LaserMode mode) {
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

		anim.SetMode (mode);
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

	private void UnaffectObject() {
		if (this.affectedObject != null) {
			switch (this.mode) {
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

	void OnDestroy() {
		UnaffectObject ();
	}
}
