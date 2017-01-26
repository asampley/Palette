using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkPlayerManager : NetworkManager {
	public PaletteColorID[] playerColors = { PaletteColorID.RED, PaletteColorID.BLUE, PaletteColorID.GREEN };

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerID) {
		PaletteColorID playerColorID = PaletteColorID.WHITE;
		if (this.numPlayers < playerColors.Length) {
			playerColorID = playerColors[this.numPlayers];
		}

		var player = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);
		Player pScript = player.GetComponent<Player> ();
		pScript.colorID = playerColorID;

		Debug.Log ("Created player with color " + playerColorID);

		NetworkServer.AddPlayerForConnection (conn, player, playerControllerID);
	}
}
