using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LaserController : NetworkBehaviour {
    //public int rotationOffset = 0; // degrees
	public GameObject laserPrefab;

	[SyncVar]
	private NetworkInstanceId laserObjID;

	private Laser laser;

    // Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		// Check that this controller is on the local player.
		if (!isLocalPlayer) return;

		// make sure the laser has been found in the scene.
		if (laser == null) {
			laser = ClientScene.FindLocalObject (laserObjID).GetComponent<Laser>();

			if (laser == null) {
				Debug.LogError ("No associated laser object");
				return;
			}
		}

		// make sure that the player has authority to modify the laser.
		if (!laser.hasAuthority) {
			Debug.LogError ("Laser " + laserObjID + " is not under player authority: Authority is " + laser.gameObject.GetComponent<NetworkIdentity>().clientAuthorityOwner);
			return;
		}

		//Debug.Log ("Controlling laser " + laserObjID);

		// Toggle the laser if the user presses the toggle button.
		if (Input.GetButtonDown ("Laser Toggle")) {
			laser.Toggle ();
		}

		// Rotate the laser based on the mouse and player position.
		rotate();
	}

	/**
	 * Spawn the laser on the local player client only.
	 */
	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer ();
	
		CmdSpawnLaser ();
	}

	[Command]
	void CmdSpawnLaser() {
		GameObject laserObj = Instantiate<GameObject>(laserPrefab);
		laserObj.name = gameObject.name + " Laser";

		NetworkServer.SpawnWithClientAuthority (laserObj, this.gameObject);

		this.laserObjID = laserObj.GetComponent<NetworkIdentity>().netId;

		Debug.Log ("Spawned laser with authority: " + connectionToClient);
	}

	void rotate() {
		Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;	// subtract pos of player from mouse pos
		difference.Normalize(); // Normalize the vector. this means that all the sum of vector will be equal to 1.

		laser.SetLaserDir(difference);
		laser.SetLaserStart (transform.position);
	}

}