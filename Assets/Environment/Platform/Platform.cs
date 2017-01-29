using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Platform : NetworkBehaviour {
	private PaletteColorID currentColorID ;

	public override void OnStartClient ()
	{
		base.OnStartClient ();

		UpdateColor ();
	}
	
	public override void OnStartServer ()
	{
		base.OnStartServer (); 

		UpdateColor ();
	}

	public void UpdateColor() {
		PaletteColor color = GetComponent<ColorAdder> ().ToPaletteColor ();
		//Debug.Log ("Set platform to " + color);

		currentColorID = color.ToID ();

		this.GetComponent<SpriteRenderer> ().color = color.ToColor();
		this.gameObject.layer = color.ToLayer ();
	}
}
