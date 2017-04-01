using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayer : MonoBehaviour {
	public Player localPlayer { get; set; }
	private Dictionary<int, Player> players;

	void Start() {
		players = new Dictionary<int, Player>();
	}

	public void SetPlayer(int i, Player o) {
		players[i] = o;
	}

	public Player GetPlayer(int i) {
		return players [i];
	}

	public void SetLocalPlayerNum(int i) {
		localPlayer.SetNumber (i);
	}
}
