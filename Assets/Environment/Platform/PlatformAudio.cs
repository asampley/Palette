using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAudio : MonoBehaviour {
	public AudioSource red;
	public AudioSource blue;
	public AudioSource green;
	public AudioSource magenta;
	public AudioSource yellow;
	public AudioSource cyan;

	public void Solo(PaletteColorID colorID) {
		red.volume = 0;
		blue.volume = 0;
		green.volume = 0;
		magenta.volume = 0;
		yellow.volume = 0;
		cyan.volume = 0;

		switch (colorID) {
		case PaletteColorID.RED:
			red.volume = 1;
			break;
		case PaletteColorID.BLUE:
			blue.volume = 1;
			break;
		case PaletteColorID.GREEN:
			green.volume = 1;
			break;
		case PaletteColorID.CYAN:
			cyan.volume = 1;
			break;
		case PaletteColorID.MAGENTA:
			magenta.volume = 1;
			break;
		case PaletteColorID.YELLOW:
			yellow.volume = 1;
			break;
		}
	}
}
