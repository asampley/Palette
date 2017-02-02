using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	[SerializeField] private float xVel = 5f;
	[SerializeField] private float jumpImpulse = 1f;

    //used for jumping animations and flipping
    public bool grounded;
    public bool isLeft;
    public bool facingRight = true;

    //private references
    private Rigidbody2D rb2d;
    private Animator anim;

    // Use this for initialization
    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) return;

		float wXVel = Input.GetAxis("Horizontal") * xVel;
		float impulse = Input.GetAxis("Vertical") * jumpImpulse;

		rb2d.velocity = new Vector2 (wXVel, rb2d.velocity.y);
		rb2d.AddForce (new Vector2 (0, impulse), ForceMode2D.Impulse);
	}
}
