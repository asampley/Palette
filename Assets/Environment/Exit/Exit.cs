using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

[RequireComponent (typeof(Collider2D))]
public class Exit : NetworkBehaviour {
	public string nextSceneName;
	List<Player> playersInExit = new List<Player>();

	public void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<Player> ()) {
			playersInExit.Add (other.GetComponent<Player> ());
		}

		if (SceneData.gameObject.GetComponent<PlayerSpawn> ().numPlayers == playersInExit.Count) {
			NetworkPlayerManager.singleton.ServerChangeScene (nextSceneName);
		}
	}

	public void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<Player> ()) {
			playersInExit.Remove (other.GetComponent<Player> ());
		}
	}
}
