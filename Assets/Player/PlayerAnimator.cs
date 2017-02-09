using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(PlayerController))]
[RequireComponent (typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour {
	private Animator anim;
	private PlayerController controller;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		controller = GetComponent<PlayerController>();
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool("Grounded", controller.grounded);
		anim.SetFloat("Horizontal Speed", Mathf.Abs(rb2d.velocity.x));
		anim.SetFloat ("Vertical Speed", Mathf.Abs(rb2d.velocity.y));
	}
}
