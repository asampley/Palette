using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {
	public int numPlayers { get { return playerSpawns.Length; } }

	public bool[] playerSpawned;

	[SerializeField] private SpawnInfo[] playerSpawns;

	void Start() {
		playerSpawned = new bool[playerSpawns.Length];
	}

	public void SetPlayerSpawned(int num, bool spawned) {
		try {
			playerSpawned[num] = true;
		} catch (IndexOutOfRangeException e) {
			Debug.LogError (e.ToString ());
		}
	}

	public bool GetPlayerSpawned(int num) {
		try {
			return playerSpawned[num];
		} catch (IndexOutOfRangeException e) {
			Debug.LogError (e.ToString ());
		}
		return false;
	}
	
	public Transform GetPlayerSpawn(int x) {
		return playerSpawns [x].spawn;
	}

	public PaletteColorID GetPlayerColorID(int x) {
		return playerSpawns [x].colorID;
	}
}
