using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;

public class NetworkConnector : MonoBehaviour {
	private NetworkPlayerManager networkManager;

	public enum Mode { HOST, CLIENT, SERVER, DISCONNECT };
	public Mode mode;

	public InputField ipSource;

	public UnityEvent onSuccess;
	public UnityEvent onFailure;

	void Start() {
		networkManager = NetworkPlayerManager.sceneObject.GetComponent<NetworkPlayerManager>();
	}

	public void AttemptConnection() {
		bool success = false;
		NetworkClient client;

		switch (mode) {
		case Mode.CLIENT:
			string fullAddress = ipSource.text;
			string[] addressSplit = fullAddress.Split (':');
			if (addressSplit.Length == 2) {
				networkManager.networkAddress = addressSplit [0];
				networkManager.networkPort = int.Parse(addressSplit [1]);
				client = networkManager.StartClient ();
				Debug.Log ("Staring client connection to " + fullAddress);
				success = true;
			} else if (addressSplit.Length == 1) {
				networkManager.networkAddress = addressSplit [0];
				client = networkManager.StartClient ();
				Debug.Log ("Staring client connection to " + fullAddress);
				success = true;
			} else {
				Debug.Log ("Malformed address: " + fullAddress);
				success = false;
			}
			break;
		case Mode.HOST:
			client = networkManager.StartHost ();
			Debug.Log ("Staring host server");
			success = true;
			break;
		case Mode.SERVER:
			success = networkManager.StartServer ();
			Debug.Log ("Staring dedicated server");
			break;
		case Mode.DISCONNECT:
			networkManager.StopClient ();
			networkManager.StopServer ();
			break;
		}

		if (success) {
			onSuccess.Invoke ();
		} else {
			onFailure.Invoke ();
		}
	}
}
