using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[ExecuteInEditMode]
public class ColorAdder : MonoBehaviour {
	[SerializeField] private PaletteColorID baseColorID = PaletteColorID.WHITE;
	[SerializeField] private bool lockColor;

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
	}

	public void RemoveAdditiveColor(PaletteColor color) {
		additiveColors.Remove (color);
		cachedColor = new PaletteColor(baseColorID);
		RecalculateCachedColor ();
	}

	public void AddSubtractiveColor(PaletteColor color) {
		subtractiveColors.Add (color);
		RecalculateCachedColor ();
	}

	public void RemoveSubtractiveColor(PaletteColor color) {
		subtractiveColors.Remove (color);
		cachedColor = new PaletteColor(baseColorID);
		RecalculateCachedColor ();
	}

	private void RecalculateCachedColor(bool invokeChangeEvent = true) {
		cachedColor = new PaletteColor(baseColorID);

		if (!lockColor) {
			foreach (PaletteColor c in additiveColors) {
				cachedColor |= c;
			}
			foreach (PaletteColor c in subtractiveColors) {
				cachedColor &= ~c;
			}

			if (invokeChangeEvent) {
				NotifyColorChange ();
			}
		}
	}

	private void NotifyColorChange() {
		colorChangeEvent.Invoke ();
	}
}
