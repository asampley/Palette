using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LaserController : NetworkBehaviour {
    //public int rotationOffset = 0; // degrees
//	public GameObject laserPrefab;

//	[SyncVar]
//	private NetworkInstanceId laserObjID;

//	private Laser _laser;
//	public Laser laser {
//		get {
//			try {
//				if (_laser == null) {
//					laser = GetComponent<Player> ().head.GetComponent<Laser> ();
//					laser.SetLaserColor (this.GetComponent<Player> ().GetColorID());
//				}
//				return _laser;
//			} catch (NullReferenceException) {
//				return null;
//			}
//		}
//		set {
//			_laser = value;
//			UpdateLaserColor ();
//		}
//	}

	public Laser laser;

	private bool isAddDown = false;
	private bool isSubDown = false;
	private bool lockLaserOn = false;

	private Vector3 laserDirection;

    // Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		// Check that this controller is on the local player.
		if (!isLocalPlayer) return;

		// make sure the laser has been found in the scene.
//		if (laser == null) {
//			laser = ClientScene.FindLocalObject (laserObjID).GetComponent<Laser>();
//
//			if (laser == null) {
//				Debug.LogError ("No associated laser object");
//				return;
//			}
//		}

		if (laser == null) {
			return;
		}

		// make sure that the player has authority to modify the laser.
		if (!laser.hasAuthority) {
			Debug.LogError ("Laser " + laser + " is not under player authority. Authority is: " + laser.GetComponent<NetworkIdentity>().clientAuthorityOwner);
			return;
		}

		//Debug.Log ("Controlling laser " + laserObjID);

		// TODO: Get rid of laser lock once testing is done
		if (Input.GetButtonDown("Laser Lock")) {
			lockLaserOn = !lockLaserOn;
		}

		// Toggle the laser if the user presses the toggle button.
		if (Input.GetButtonDown ("Laser Add")) {
			isAddDown = true;
			GetComponent<PlayerAudio>().laserOnSource.Play ();
		}
		if (Input.GetButtonUp ("Laser Add")) {
			isAddDown = false;
		}

		// Turn laser on with subtract mode if user presses button.
		if (Input.GetButtonDown ("Laser Subtract")) {
			isSubDown = true;
			GetComponent<PlayerAudio>().laserOnSource.Play ();
		}
		if (Input.GetButtonUp ("Laser Subtract")) {
			isSubDown = false;
		}

		//Code for action on mouse moving
		//print("Mouse moved left");
		// change laser if needs changing
		if (isAddDown) {
			laser.SetLaserOn (true);
			laser.SetLaserMode (LaserMode.ADD);
		} else if (isSubDown) {
			laser.SetLaserOn (true);
			laser.SetLaserMode (LaserMode.SUBTRACT);
		} else if (lockLaserOn) {
			laser.SetLaserOn (true);
		} else {
			laser.SetLaserOn (false);
		}

		// If mouse is moved
		float deadzone = 0.05f;
		if ((Input.GetAxis("Mouse X") > deadzone) || (Input.GetAxis("Mouse X") < -deadzone) 
			|| (Input.GetAxis("Mouse Y") > deadzone) || (Input.GetAxis("Mouse Y") < -deadzone)) {
			// Rotate the laser based on the mouse and player position.
			rotate();
		}
			
		// if user changes direction while keeping mouse still, need to call rotate.

	}

	/**
	 * Spawn the laser on the local player client only.
	 */
//	public override void OnStartLocalPlayer ()
//	{
//		base.OnStartLocalPlayer ();
//	
//		CmdSpawnLaser ();
//	}

//	[Command]
//	void CmdSpawnLaser() {
//		GameObject laserObj = Instantiate<GameObject>(laserPrefab);
//		laserObj.name = gameObject.name + " Laser";
//
//		NetworkServer.SpawnWithClientAuthority (laserObj, this.gameObject);
//
//		this.laserObjID = laserObj.GetComponent<NetworkIdentity>().netId;
//
//		UpdateLaserColor ();
//
//		//Debug.Log ("Spawned laser with authority: " + connectionToClient);
//	}

	public void UpdateLaserColor() {
		if (laser != null) {
			// set the color to match the player
			laser.SetLaserColor (this.GetComponent<Player> ().GetColorID ());
		}
	}

	void rotate() {
		Vector3 difference = Vector3.zero;
		if (Input.GetJoystickNames ().Length == 0) {
			// subtract pos of player from mouse pos
			difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - GetComponent<Player> ().head.transform.position;
			//difference.x = Input.GetAxis ("Mouse X"); // this doesn't work
			//difference.y = -Input.GetAxis ("Mouse Y");
		} else {
			difference.x = Input.GetAxis ("Light Horizontal");
			difference.y = -Input.GetAxis ("Light Vertical");
			Debug.Log (difference);
		}

		difference.z = 0;
		difference.Normalize(); // Normalize the vector. this means that all the sum of vector will be equal to 1.

		if (Vector3.Distance(difference, Vector3.zero) < 0.2) {
			return;
		}

		laser.rb2d.transform.rotation = Quaternion.Euler(new Vector3(difference.x, difference.y, Mathf.Rad2Deg * Mathf.Atan2(difference.y, difference.x)));
	}

}