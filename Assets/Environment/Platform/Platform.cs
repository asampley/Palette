using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(ColorAdder))]
public class Platform : NetworkBehaviour, PlayerColorListener {
	private PaletteColorID currentColorID;

	public override void OnStartClient ()
	{
		base.OnStartClient ();

		SceneData.sceneObject.GetComponent<PlayerColor> ().AddListener (this);
		UpdateColor ();
	}

	public void UpdateColor(bool forceUpdate = false) {
		PaletteColorID previousColorID = currentColorID;

		PaletteColor color = GetComponent<ColorAdder> ().ToPaletteColor ();

		currentColorID = color.ToID ();

		if (currentColorID == previousColorID && !forceUpdate) return;
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

		foreach (PlatformBit platformBit in this.GetComponent<PlatformGenerator>().GetPlatformBits()) {
			platformBit.GetComponent<SpriteRenderer> ().color = rgbColor;
		}
	}

	public void OnLocalPlayerColorChange() {
		UpdateColor ();
	}
}
