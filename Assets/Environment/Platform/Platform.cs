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

	public void UpdateColor() {
		PaletteColorID previousColorID = currentColorID;

		PaletteColor color = GetComponent<ColorAdder> ().ToPaletteColor ();
		//Debug.Log ("Set platform to " + color);

		currentColorID = color.ToID ();

		if (currentColorID == previousColorID) return;
		// only continue past this point if the color has changed

		this.gameObject.layer = color.ToLayer ();

		// set to be dark if it matches the player color
		Color rgbColor;
		if (SceneData.sceneObject.GetComponent<PlayerColor> ().GetLocalPlayerColor () == currentColorID) {
			rgbColor = color.ToColorDark ();
		} else {
			rgbColor = color.ToColor ();
		}

		foreach (GameObject platformBit in this.GetComponent<PlatformGenerator>().GetPlatformBits()) {
			platformBit.GetComponent<SpriteRenderer> ().color = rgbColor;
		}
	}

	public void OnLocalPlayerColorChange() {
		UpdateColor ();
	}
}
