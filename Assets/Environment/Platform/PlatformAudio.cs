﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAudio : MonoBehaviour {
	public AudioSource red;
	public AudioSource blue;
	public AudioSource green;
	public AudioSource magenta;
	public AudioSource yellow;
	public AudioSource cyan;

	public float volume;

	public void Mute() {
		red.volume = 0;
		blue.volume = 0;
		green.volume = 0;
		magenta.volume = 0;
		yellow.volume = 0;
		cyan.volume = 0;
	}

	public void Solo(PaletteColorID colorID) {
		Mute ();

		switch (colorID) {
		case PaletteColorID.RED:
			red.volume = volume;
			break;
		case PaletteColorID.BLUE:
			blue.volume = volume;
			break;
		case PaletteColorID.GREEN:
			green.volume = volume;
			break;
		case PaletteColorID.CYAN:
			cyan.volume = volume;
			break;
		case PaletteColorID.MAGENTA:
			magenta.volume = volume;
			break;
		case PaletteColorID.YELLOW:
			yellow.volume = volume;
			break;
		}
	}
}
