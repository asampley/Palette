using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class GhostPlayer : NetworkBehaviour {
	[SyncVar (hook="OnSelectPlayerNum")]
	public int selectedPlayerNum = 0;

	public UnityEvent<int> onSelect;

	public void SelectPlayerNum (int playerNum) {

	}

	[Command]
	void CmdSelectPlayerNum(int playerNum) {

	}


	void OnSelectPlayerNum(int playerNum) {
		if (!hasAuthority) {
			onSelect.Invoke (playerNum);
		}
	}
}
