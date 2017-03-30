using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class PlayerColorChanger : MonoBehaviour {
	public PaletteColorID color;

	void OnTriggerEnter2D(Collider2D other) {
		Player player = other.GetComponent<Player> ();

		if (player != null) {
			player.SetColor (color);
		}
	}
}
