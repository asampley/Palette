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
	public void SpawnPlayer (NetworkConnection conn, short playerControllerID) {

		//NetworkServer.SpawnWithClientAuthority (playerPrefab, conn);
		GameObject player = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);

		try {
		} catch (IndexOutOfRangeException e) {
			Debug.LogError (e.ToString());
		}

		player.name = "Player " + this.numPlayers;

		//Debug.Log ("Created player with color " + playerColorID);

		NetworkServer.AddPlayerForConnection (conn, player, playerControllerID);
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
	{
		this.SpawnPlayer (conn, playerControllerId);
	}

//	public override void OnServerConnect(NetworkConnection conn) {
//		base.OnServerConnect (conn);
//		
//		if (SceneData.sceneObject != null) {
//			this.SpawnPlayer (conn, 0);
//		}
//	}

//	public override void OnServerReady(NetworkConnection conn) {
//		base.OnServerReady (conn);
//	}
//
//	public override void OnServerSceneChanged (string sceneName)
//	{
//		base.OnServerSceneChanged (sceneName);
//		
//		if (SceneData.sceneObject != null) {
//			foreach (NetworkConnection conn in NetworkServer.connections) {
//				this.SpawnPlayer (conn, 0);
//			}
//		}
//	}
    
    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
		base.OnServerError (conn, errorCode);
        print(conn.lastError + " Error Code " + errorCode);
    }
    public override void OnClientError(NetworkConnection conn, int errorCode)
	{
		base.OnServerError (conn, errorCode);
        print(conn.lastError + " Error Code " + errorCode);
    }
}
