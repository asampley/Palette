using System;
using UnityEngine;

public class PaletteColor {
	public static readonly PaletteColor BLACK = new PaletteColor(0x0);
	public static readonly PaletteColor RED = new PaletteColor(0x1);
	public static readonly PaletteColor GREEN = new PaletteColor(0x2);
	public static readonly PaletteColor BLUE = new PaletteColor(0x4);
	public static readonly PaletteColor MAGENTA = RED | BLUE;
	public static readonly PaletteColor YELLOW = RED | GREEN;
	public static readonly PaletteColor CYAN = GREEN | BLUE;
	public static readonly PaletteColor WHITE = RED | GREEN | BLUE;

	private int color;

	private PaletteColor(int color) {
		this.color = color & 0x7;
	}

	public PaletteColor(PaletteColorID id) {
		this.color = (int)id;
	}

	public static PaletteColor operator & (PaletteColor first, PaletteColor second) {
		return new PaletteColor(first.color & second.color);
	}

	public static PaletteColor operator | (PaletteColor first, PaletteColor second) {
		return new PaletteColor(first.color | second.color);
	}

	public static PaletteColor operator ~ (PaletteColor col) {
		return new PaletteColor (~col.color);
	}

	/// <summary>
	/// Guaranteed to go from 0 to 7
	/// </summary>
	/// <returns>The int.</returns>
	public int ToInt() {
		return this.color;
	}

	public PaletteColorID ToID() {
		return (PaletteColorID)(this.color);
	}

	public static PaletteColor FromInt(int i) {
		return new PaletteColor (i);
	}

	public Color ToColor() {
		if (this.color == 0) {
			return new Color (0f, 0f, 0f);
		} else {
			return new Color (
				(RED & this).color != 0 ? 1f : 0.5f,
				(GREEN & this).color != 0 ? 1f : 0.5f,
				(BLUE & this).color != 0 ? 1f : 0.5f
			);
		}
	}

	public Color ToColorDark() {
		if (this.color == 0) {
			return new Color (0f, 0f, 0f);
		} else {
			return new Color (
				(RED & this).color != 0 ? 0.2f : 0f,
				(GREEN & this).color != 0 ? 0.2f : 0f,
				(BLUE & this).color != 0 ? 0.2f : 0f
				);
		}
	}

	public int ToLayer() {
		return LayerMask.NameToLayer (ToLayerName());
	}

	public string ToLayerName() {
		return this.ToString ();
	}

	public int ToEntityLayer() {
		return LayerMask.NameToLayer (ToEntityLayerName());
	}

	public string ToEntityLayerName() {
		return this.ToString () + " Entity";
	}

	public static PaletteColor RandomColor() {
		System.Random rand = new System.Random ();
		return new PaletteColor(rand.Next (8));
	}

	public override string ToString ()
	{
		switch (color) {
		case 0:
			return "Black";
		case 1:
			return "Red";
		case 2:
			return "Green";
		case 3:
			return "Yellow";
		case 4:
			return "Blue";
		case 5:
			return "Magenta";
		case 6:
			return "Cyan";
		case 7:
			return "White";
		default:
			return "Unknown Color";
		}
	}

	public override bool Equals (object obj)
	{
		if (obj is PaletteColor) {
			return ((PaletteColor)obj).color == this.color;
		}
		return false;
	}

	public override int GetHashCode ()
	{
		return color;
	}
}

/// <summary>
/// Guaranteed to go from 0 to 7.
/// </summary>
public enum PaletteColorID {
	BLACK = 0,
	RED = 1,
	GREEN = 2,
	BLUE = 4,
	YELLOW = 3,
	CYAN = 6,
	MAGENTA = 5,
	WHITE = 7
}

