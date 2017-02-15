using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyButton : MonoBehaviour {
	public int playerNum;

	// Use this for initialization
	void Start () {
		this.GetComponent<Image>().color = new PaletteColor(SceneData.sceneObject.GetComponent<PlayerSpawn> ().GetPlayerColorID (playerNum)).ToColor();
	}
}
