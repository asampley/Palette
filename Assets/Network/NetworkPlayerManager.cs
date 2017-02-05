using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkPlayerManager : NetworkManager {

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerID) {
		PaletteColorID playerColorID = PaletteColorID.WHITE;

		try {
			playerColorID = GameObject.Find ("Client Data").GetComponent<PlayerColor> ().GetPlayerColor (this.numPlayers);
		} catch (IndexOutOfRangeException e) {
			// It's cool, the color will just be white (which may not be cool).
		}

		//NetworkServer.SpawnWithClientAuthority (playerPrefab, conn);
		GameObject player = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);

		player.name = "Player " + this.numPlayers; // starts with 0
		Player pScript = player.GetComponent<Player> ();
		pScript.colorID = playerColorID;

		//Debug.Log ("Created player with color " + playerColorID);

		NetworkServer.AddPlayerForConnection (conn, player, playerControllerID);
	}
}
