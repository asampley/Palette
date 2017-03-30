using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[SelectionBase]
[RequireComponent (typeof(ColorAdder))]
public class Platform : NetworkBehaviour, PlayerColorListener {
	private PaletteColorID currentColorID;
	private PaletteColorID playerColorID;
	private MaterialPropertyBlock _materialPB;
	private MaterialPropertyBlock materialPB {
		get {
			if (_materialPB == null) {
				_materialPB = new MaterialPropertyBlock ();
			}
			return _materialPB;
		}
	}

	public override void OnStartClient ()
	{
		base.OnStartClient ();

		SceneData.sceneObject.GetComponent<PlayerColor> ().AddListener (this);
		UpdateColor ();
	}

	public void UpdateColor(bool forceUpdate = false) {
		PaletteColorID previousColorID = currentColorID;
		PaletteColorID previousPlayerColorID = playerColorID;

		PaletteColor color = GetComponent<ColorAdder> ().ToPaletteColor();
		currentColorID = color.ToID ();
		playerColorID = SceneData.sceneObject.GetComponent<PlayerColor> ().GetLocalPlayerColor ();

		if (currentColorID == previousColorID && playerColorID == previousPlayerColorID && !forceUpdate) return;
		// only continue past this point if the color has changed, or if we are forced
		//Debug.Log ("Set platform to " + color);

		this.gameObject.layer = color.ToLayer ();

		// set to be dark if it matches the player color
		Color rgbColor;
		if (SceneData.sceneObject.GetComponent<PlayerColor> ().GetLocalPlayerColor () == currentColorID) {
			rgbColor = color.ToColorDark ();
		} else {
			rgbColor = color.ToColor ();
		}

		if (GetComponent<PlatformAudio> () != null) {
			if (color.ToID () != GetComponent<ColorAdder> ().GetBaseColorID ()) {
				GetComponent<PlatformAudio> ().Solo (color.ToID ());
			} else {
				GetComponent<PlatformAudio> ().Mute ();
			}
		}

		Renderer rend = GetComponent<Renderer> ();
		materialPB.SetColor ("_Color", rgbColor);
		rend.SetPropertyBlock (materialPB);
	}

	public void OnLocalPlayerColorChange() {
		UpdateColor ();
	}
}
