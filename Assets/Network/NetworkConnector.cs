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
                Debug.Log ("Staring client connection to " + fullAddress);
				client = networkManager.StartClient ();
				success = client != null;
			} else if (addressSplit.Length == 1) {
				networkManager.networkAddress = addressSplit [0];
                Debug.Log("Staring client connection to " + fullAddress);
                client = networkManager.StartClient ();
				success = client != null;
			} else {
				Debug.Log ("Malformed address: " + fullAddress);
				success = false;
			}
			break;
		case Mode.HOST:
            Debug.Log("Starting host server");
            client = networkManager.StartHost();
            success = client != null;
            break;
		case Mode.SERVER:
            Debug.Log("Staring dedicated server");
            success = networkManager.StartServer ();
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
