using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlatformMover : MonoBehaviour {
	public Vector2 start;
	public Vector2 end;
	private float dist;
	public float speed = 0f;
	private const float eps = 0.01f;

	void Start() {
		dist = Vector2.Distance (start, end) - eps; // reduce by epsilon, to remove possibility of floating point problems
	}

	void FixedUpdate() {
		if (speed == 0f) return;
//		Debug.Log (speed);

		float distFromStart = Vector2.Distance (start, this.transform.position) - dist;
		float distFromEnd = Vector2.Distance (end, this.transform.position) - dist;

//		Debug.Log("Distance: " + dist + " From Start: " + Vector2.Distance (start, this.transform.position) + " From End: " + Vector2.Distance (end, this.transform.position));

		// if farther from start than end, move towards start
		if (0 <= distFromStart) {
			Rigidbody2D rb = this.GetComponent<Rigidbody2D> ();
			rb.velocity = (start - end).normalized * speed;
//			Debug.Log ("Set vel to " + rb.velocity);
		}

		// if farther from end than start, move towards end
		if (0 <= distFromEnd) {
			Rigidbody2D rb = this.GetComponent<Rigidbody2D> ();
			rb.velocity = (end - start).normalized * speed;
//			Debug.Log ("Set vel to " + rb.velocity);
		}
	}
}
