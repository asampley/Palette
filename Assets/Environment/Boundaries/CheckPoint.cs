using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class CheckPoint : MonoBehaviour {
	public bool disableOnReach = true;
	public Transform[] spawns;
	private bool[] playerEntered;

	void Start() {
		playerEntered = new bool[spawns.Length];
	}

	void OnTriggerEnter2D(Collider2D other) {
		Player player = other.GetComponent<Player> ();

		if (player != null) {
			playerEntered [player.GetNumber ()] = true;

			// set all players spawns, if all players have entered at some point
			// then reset for future use if set to.
			if (Array.TrueForAll (playerEntered, x => x)) {
				for (int i = 0; i < spawns.Length; ++i) {
					SceneData.sceneObject.GetComponent<LocalPlayer> ().GetPlayer (i).spawn = spawns [i];

					if (!disableOnReach) {
						playerEntered [i] = false;
					}
				}

				if (disableOnReach) {
					this.enabled = false;
				}
			}
		}
	}
}
