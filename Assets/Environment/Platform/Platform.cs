using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Platform : NetworkBehaviour, PlayerColorListener {
	private PaletteColorID currentColorID ;

	public override void OnStartClient ()
	{
		base.OnStartClient ();

		GameObject.Find ("Client Data").GetComponent<PlayerColor> ().AddListener (this);
		UpdateColor ();
	}

	public void UpdateColor() {
		PaletteColor color = GetComponent<ColorAdder> ().ToPaletteColor ();
		//Debug.Log ("Set platform to " + color);

		currentColorID = color.ToID ();

		this.GetComponent<SpriteRenderer> ().color = color.ToColor();
		this.gameObject.layer = color.ToLayer ();

		// set to be invisible if it matches the player color
		this.GetComponent<SpriteRenderer> ().enabled = GameObject.Find ("Client Data").GetComponent<PlayerColor> ().GetPlayerColor() != currentColorID;
	}

	public void OnPlayerColorChange() {
		UpdateColor ();
	}
}
