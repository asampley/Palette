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

	public PaletteColor(int color) {
		this.color = color;
	}

	public static PaletteColor operator & (PaletteColor first, PaletteColor second) {
		return new PaletteColor(first.color & second.color);
	}

	public static PaletteColor operator | (PaletteColor first, PaletteColor second) {
		return new PaletteColor(first.color | second.color);
	}

	public int ToInt() {
		return this.color;
	}

	public static PaletteColor FromInt(int i) {
		return new PaletteColor (i & 0x7);
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

	public static PaletteColor RandomColor() {
		System.Random rand = new System.Random ();
		return new PaletteColor(rand.Next (8));
	}
}

