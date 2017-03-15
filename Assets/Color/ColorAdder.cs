using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[ExecuteInEditMode]
public class ColorAdder : MonoBehaviour {
	[SerializeField] private PaletteColorID baseColorID;

	private List<PaletteColor> additiveColors = new List<PaletteColor>();
	private List<PaletteColor> subtractiveColors = new List<PaletteColor> ();
	private PaletteColor cachedColor;

	public UnityEvent colorChangeEvent;

	void OnEnable() {
		cachedColor = new PaletteColor(baseColorID);
		NotifyColorChange ();
	}

	public void SetBaseColorID(PaletteColorID id) {
		baseColorID = id;
		RecalculateCachedColor ();
		NotifyColorChange ();
	}

	public PaletteColorID GetBaseColorID() {
		return baseColorID;
	}

	// Use this for initialization
//	public override void OnStartClient() {
//		cachedColor = new PaletteColor (baseColorID);
//		NotifyColorChange ();
//	}
//
//	public override void OnStartServer() {
//		cachedColor = new PaletteColor (baseColorID);
//		NotifyColorChange ();
//	}

	public PaletteColor ToPaletteColor() {
		return cachedColor;
	}

	public void AddAdditiveColor(PaletteColor color) {
		additiveColors.Add (color);
		RecalculateCachedColor ();

		NotifyColorChange ();
	}

	public void RemoveAdditiveColor(PaletteColor color) {
		additiveColors.Remove (color);
		cachedColor = new PaletteColor(baseColorID);
		RecalculateCachedColor ();

		NotifyColorChange ();
	}

	public void AddSubtractiveColor(PaletteColor color) {
		subtractiveColors.Add (color);
		RecalculateCachedColor ();

		NotifyColorChange ();
	}

	public void RemoveSubtractiveColor(PaletteColor color) {
		subtractiveColors.Remove (color);
		cachedColor = new PaletteColor(baseColorID);
		RecalculateCachedColor ();

		NotifyColorChange ();
	}

	private void RecalculateCachedColor() {
		cachedColor = new PaletteColor(baseColorID);
		foreach (PaletteColor c in additiveColors) {
			cachedColor |= c;
		}
		foreach (PaletteColor c in subtractiveColors) {
			cachedColor &= ~c;
		}
	}

	private void NotifyColorChange() {
		colorChangeEvent.Invoke ();
	}
}
