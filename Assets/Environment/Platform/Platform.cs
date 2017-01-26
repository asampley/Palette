using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Platform : NetworkBehaviour {
	public PaletteColorID baseColorID;

	[SyncVar (hook="OnColorChange")]
	private PaletteColorID currentColorID ;

	// Use this for initialization
	void Start () {
		currentColorID = baseColorID;
		OnColorChange (baseColorID);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnColorChange(PaletteColorID newColor) {
		PaletteColor color = new PaletteColor(newColor);

		this.GetComponent<SpriteRenderer> ().color = color.ToColor();
		this.gameObject.layer = color.ToLayer ();
	}
}
