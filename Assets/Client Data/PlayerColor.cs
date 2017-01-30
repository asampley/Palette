using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour, Listenable<PlayerColorListener> {
	private PaletteColorID playerColor; // updated by the Player script
	private List<PlayerColorListener> listeners = new List<PlayerColorListener>();

	// Sets local player color, does not change player.
	public void SetPlayerColor(PaletteColorID id) {
		this.playerColor = id;

		ForEachListener ((x) => x.OnPlayerColorChange ());
	}

	public PaletteColorID GetPlayerColor() {
		return playerColor;
	}

	public void AddListener (PlayerColorListener listener) {
		listeners.Add (listener);
	}
	public void RemoveListener(PlayerColorListener listener) {
		listeners.RemoveAll ((other) => other.Equals (listener));
	}

	public void ForEachListener (Action<PlayerColorListener> action) {
		foreach (PlayerColorListener listener in listeners) {
			action.Invoke (listener);
		}
	}
}

public interface PlayerColorListener : Listener {
	void OnPlayerColorChange();
}
