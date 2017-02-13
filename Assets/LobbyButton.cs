using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class LobbyButton : NetworkBehaviour {
    NetworkManager nm;
	public Text playerName;
	[SyncVar(hook = "OnPlayerIDChanged")] public string playerID;
    public Button button;

	void Start() {
		nm = NetworkPlayerManager.sceneObject.GetComponent<NetworkPlayerManager> ();
	}

    [Command]
    void CmdSetPlayerID(string newID)
    {
        playerID = newID;
    }
    
    public void SelectPlayer ()
    {
        // string myPlayerID = string.Format("Player {0}", netId.Value);
        string myPlayerID = string.Format("Player {0}", nm.numPlayers);        
        CmdSetPlayerID(myPlayerID);
        Debug.Log(myPlayerID + " has clicked");
	}
	
    public override void OnStartClient ()
    {
        OnPlayerIDChanged(playerID);
    }
 
    void OnPlayerIDChanged(string newValue)
    {
		playerName.text = newValue;
	}

}
