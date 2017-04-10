using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(PlayerController))]
[RequireComponent (typeof(Player))]
[RequireComponent (typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour {
	private Animator anim;
	private Player player;
	private PlayerController controller;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		player = GetComponent<Player> ();
		controller = GetComponent<PlayerController>();
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool("Grounded", controller.grounded);
		anim.SetFloat("Horizontal Speed", Mathf.Abs(rb2d.velocity.x - controller.parentVel.x));
		anim.SetFloat ("Vertical Speed", Mathf.Abs(rb2d.velocity.y - controller.parentVel.y));

		if (player.head != null) {
			// flip player head based on rotation.
			player.head.GetComponent<SpriteRenderer> ().flipY = Mathf.Cos (Mathf.Deg2Rad * player.head.transform.rotation.eulerAngles.z) < 0;
			Vector3 headScale = player.head.transform.localScale;
			headScale.x = controller.facingRight ? 1 : -1;
			player.head.transform.localScale = headScale;
			//player.head.GetComponent<SpriteRenderer> ().flipX = !controller.facingRight;
		}
	}
}
