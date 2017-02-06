using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {
	[SerializeField] private Transform[] playerSpawns;

	public Transform GetPlayerSpawn(int x) {
		return playerSpawns [x];
	}
}
