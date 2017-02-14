using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class LobbyPanel : MonoBehaviour {
    public void Refresh() {
		int playerNum = SceneData.sceneObject.GetComponent<LocalPlayer> ().localPlayer.GetComponent<Player>().GetNumber ();
		int numPlayers = SceneData.sceneObject.GetComponent<PlayerSpawn> ().numPlayers;
		if (playerNum >= 0 && playerNum < numPlayers) {
			this.gameObject.SetActive (false);
		}
	}
}
