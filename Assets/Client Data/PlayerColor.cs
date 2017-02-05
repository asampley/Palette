using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour, Listenable<PlayerColorListener> {
	[SerializeField] private PaletteColorID[] levelPlayerColors;

	private PaletteColorID localPlayerColor; // updated by the Player script
	private List<PlayerColorListener> listeners = new List<PlayerColorListener>();

	/// Get the color for player x.
	/// <exception cref="IndexOutOfRangeException">No such player is defined.</exception>
	public PaletteColorID GetPlayerColor(int x) {
		return levelPlayerColors [x];
	}

	// Sets local player color, does not change player.
	public void SetLocalPlayerColor(PaletteColorID id) {
		this.localPlayerColor = id;

		ForEachListener ((x) => x.OnLocalPlayerColorChange ());
	}

	public PaletteColorID GetLocalPlayerColor() {
		return localPlayerColor;
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
	void OnLocalPlayerColorChange();
}
