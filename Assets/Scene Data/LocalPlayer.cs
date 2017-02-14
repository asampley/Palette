using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayer : MonoBehaviour {
	public GameObject localPlayer { get; set; }

	public void SetNumber(int number) {
		localPlayer.GetComponent<Player>().SetNumber (number);
	}
}
