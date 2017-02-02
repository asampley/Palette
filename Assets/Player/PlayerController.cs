using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	[SerializeField] private float xVel = 5f;
	[SerializeField] private float yVel = 5f;

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

		float dx = Input.GetAxis("Horizontal") * xVel * Time.deltaTime;
		float dy = Input.GetAxis("Vertical") * yVel * Time.deltaTime;

		transform.Translate(dx, dy, 0);
	}
}
