using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class NoLeaveBoundary : MonoBehaviour {
	void OnTriggerExit2D(Collider2D other) {
		Player p = other.GetComponent<Player> ();
		if (p != null) {
			p.Respawn ();
		}
	}
}
