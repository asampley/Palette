using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {
	public int numPlayers { get { return playerSpawns.Length; } }

	[SerializeField] private SpawnInfo[] playerSpawns;
	
	public Transform GetPlayerSpawn(int x) {
		return playerSpawns [x].spawn;
	}

	public PaletteColorID GetPlayerColorID(int x) {
		return playerSpawns [x].colorID;
	}
}
