using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public enum PlayerSelect { PLAYER1, PLAYER2 }

public class LobbyButton : NetworkBehaviour {
    
	[SyncVar (hook="OnPlayerSelect")]
	private List<PlayerSelect> player;
    
    void OnClick(PlayerSelect player) {
            UpdatePlayerSelect (player);
    }
    
    void UpdatePlayerSelect(PlayerSelect player) {

        
    }

}
