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

	/// <summary>
	/// Use this function to create the player objects, rather than the default method.
	/// </summary>
	/// <param name="conn">Conn.</param>
	/// <param name="colorID">Color.</param>
	public void SpawnPlayer (NetworkConnection conn, short playerControllerID, int playerNum) {

		//NetworkServer.SpawnWithClientAuthority (playerPrefab, conn);
		GameObject player = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);

		try {
			PlayerSpawn info = SceneData.gameObject.GetComponent<PlayerSpawn>();
			player.transform.position = info.GetPlayerSpawn(playerNum).position;
			Player pScript = player.GetComponent<Player> ();
			pScript.colorID = info.GetPlayerColorID(playerNum);
		} catch (IndexOutOfRangeException e) {
			Debug.LogError (e.ToString());
		}

		player.name = "Player " + playerNum; // starts with 0

		//Debug.Log ("Created player with color " + playerColorID);

		NetworkServer.AddPlayerForConnection (conn, player, playerControllerID);
	}

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		this.SpawnPlayer(conn, playerControllerId, 0);
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerID, NetworkReader extraMessage) {
		int playerNum = extraMessage.ReadInt32 ();

		this.SpawnPlayer (conn, playerControllerID, playerNum);
	}
}
