using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class ColorAdder : NetworkBehaviour {

	[SyncVar]
	public PaletteColorID baseColorID;

	private List<PaletteColor> additiveColors = new List<PaletteColor>();
	private PaletteColor cachedColor;

	public UnityEvent colorChangeEvent;

	// Use this for initialization
	public override void OnStartClient() {
		cachedColor = new PaletteColor (baseColorID);
		NotifyColorChange ();
	}

	public override void OnStartServer() {
		cachedColor = new PaletteColor (baseColorID);
		NotifyColorChange ();
	}

	public PaletteColor ToPaletteColor() {
		return cachedColor;
	}

	public void AddColor(PaletteColor color) {
		additiveColors.Add (color);
		cachedColor |= color;

		NotifyColorChange ();
	}

	public void RemoveColor(PaletteColor color) {
		additiveColors.Remove (color);
		cachedColor = new PaletteColor(baseColorID);
		foreach (PaletteColor c in additiveColors) {
			cachedColor |= c;
		}

		NotifyColorChange ();
	}
	/*
	public void SetListener(NetworkBehaviour listener) {
		this.listener = listener.netId;
		NotifyColorChange ();
	}

	private void NotifyColorChange() {
		Debug.Log ("Notifying " + ClientScene.FindLocalObject (listener) + " of color change to " + this.ToPaletteColor());
		ClientScene.FindLocalObject(listener).GetComponent<ColorAdderListener>().OnColorChange(ToPaletteColor());
	}
	*/

	private void NotifyColorChange() {
		string s = "";
		foreach (PaletteColor c in additiveColors) {
			s += c + " ";
		}
		Debug.Log ("List of colors: " + s);

		colorChangeEvent.Invoke ();
	}
}

public interface ColorAdderListener {
	void OnColorChange(PaletteColor color);
}
