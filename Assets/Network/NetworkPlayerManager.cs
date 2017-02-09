using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkPlayerManager : NetworkManager {
	private const string TAG = "Network Manager";

	public static GameObject sceneObject {
		get {
			return GameObject.FindWithTag (TAG);
		}
	}

	void Start() {
		if (!this.gameObject.CompareTag(TAG)) {
			Debug.LogError ("Tag of object " + this.gameObject.name + " is not " + TAG);
		}
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerID) {
		PaletteColorID playerColorID = PaletteColorID.WHITE;

		Debug.Log (playerControllerID);

		try {
			playerColorID = SceneData.gameObject.GetComponent<PlayerSpawn> ().GetPlayerColorID (this.numPlayers);
		} catch (IndexOutOfRangeException e) {
			// It's cool, the color will just be white (which may not be cool).
		}

		//NetworkServer.SpawnWithClientAuthority (playerPrefab, conn);
		GameObject player = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);

		try {
			player.transform.position = SceneData.gameObject.GetComponent<PlayerSpawn> ().GetPlayerSpawn(this.numPlayers).position;
		} catch (IndexOutOfRangeException e) {

		}

		player.name = "Player " + this.numPlayers; // starts with 0
		Player pScript = player.GetComponent<Player> ();
		pScript.colorID = playerColorID;

		//Debug.Log ("Created player with color " + playerColorID);

		NetworkServer.AddPlayerForConnection (conn, player, playerControllerID);
	}
}
