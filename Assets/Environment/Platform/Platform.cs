using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(ColorAdder))]
public class Platform : NetworkBehaviour, PlayerColorListener {
	private PaletteColorID currentColorID ;

	public override void OnStartClient ()
	{
		base.OnStartClient ();

		SceneData.sceneObject.GetComponent<PlayerColor> ().AddListener (this);
		UpdateColor ();
	}

	public void UpdateColor() {
		PaletteColor color = GetComponent<ColorAdder> ().ToPaletteColor ();
		//Debug.Log ("Set platform to " + color);

		currentColorID = color.ToID ();
		this.gameObject.layer = color.ToLayer ();

		// set to be dark if it matches the player color
		if (SceneData.sceneObject.GetComponent<PlayerColor> ().GetLocalPlayerColor () == currentColorID) {
			this.GetComponent<SpriteRenderer> ().color = color.ToColorDark ();
		} else {
			this.GetComponent<SpriteRenderer> ().color = color.ToColor ();
		}
	}

	public void OnLocalPlayerColorChange() {
		UpdateColor ();
	}
}
