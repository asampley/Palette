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
    private void Update()
    {
        Dictionary<int, Player> allPlayers = SceneData.sceneObject.GetComponent<LocalPlayer>().GetPlayers();
        foreach (KeyValuePair<int, Player> player in allPlayers)
        {
            if (player.Key == playerNum)
            {
                this.GetComponent<Button>().enabled = false;
                this.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
            }
        } 
        
    }
}
