using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkPlayerManager : NetworkManager {
	public PaletteColor playerColor = PaletteColor.BLUE;

	public override void OnClientConnect (NetworkConnection conn) {
		IntegerMessage message = new IntegerMessage (PaletteColor.RandomColor().ToInt());

		ClientScene.AddPlayer (conn, 0, message);
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerID, NetworkReader extraMessageReader) {

		var stream = extraMessageReader.ReadMessage<IntegerMessage> ();
		playerColor = PaletteColor.FromInt(stream.value);

		var player = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);
		Player pScript = player.GetComponent<Player> ();
		pScript.colorID = playerColor.ToID();

		//Debug.Log ("Created player with color " + playerColor.ToInt());

		NetworkServer.AddPlayerForConnection (conn, player, playerControllerID);
	}
}
