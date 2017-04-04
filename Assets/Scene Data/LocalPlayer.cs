using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayer : MonoBehaviour {
	public Player localPlayer { get; set; }
    private Dictionary<int, Player> players;

	void Start() {
		players = new Dictionary<int, Player>();
	}

	public void SetPlayer(int i, Player o) {
        if (o != null)
        {
            players[i] = o;
        } else
        {
            players.Remove(i);
        }
       
	}

	public Player GetPlayer(int i) {
		return players [i];
	}

	public void SetLocalPlayerNum(int i) {
		localPlayer.SetNumber (i);
	}

    public Dictionary<int, Player> GetPlayers()
    {
        return players;
    }
}
