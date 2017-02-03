using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	[SerializeField] private float xVel = 5f;
	[SerializeField] private float yVel = 5f;
    [SerializeField] private float maxspeed = 10f;

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

        //flips player
        if (Input.GetKeyDown("d") || Input.GetKeyDown("right"))
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (Input.GetButtonDown("Jump")){
            if (grounded)
            {
                rb2d.AddForce(Vector2.up * yVel);
            }
        }

        //float dx = Input.GetAxis("Horizontal") * xVel * Time.deltaTime;
        //float dy = Input.GetAxis("Vertical") * yVel * Time.deltaTime;

            //transform.Translate(dx, dy, 0);
    }

    void FixedUpdate()
    {
        Vector3 easeVelocity = rb2d.velocity;
        //doesnt affect y
        easeVelocity.y = rb2d.velocity.y;
        //z axis not used in 2d
        easeVelocity.z = 0.0f;
        //multiplies the easevelocity.x by 0.75 which will reduce the exit velocity, reducing speed
        easeVelocity.x *= 3f;


        //takes left and right arrows or a and d as input 1 for right and -1 for left
        float h = Input.GetAxis("Horizontal");

        //fake friction/easing x speed of player

        if (grounded)
        {
            rb2d.velocity = easeVelocity;
        }

        //this should move the player based on what we input (vector2 is x axis)
        if (grounded)
        {
            rb2d.AddForce((Vector2.right * xVel) * h);
        }
        else
        {
            rb2d.AddForce((Vector2.right * xVel / 2) * h);
        }


        //limits speed of player
        if (rb2d.velocity.x > maxspeed)
        {
            rb2d.velocity = new Vector2(maxspeed, rb2d.velocity.y);
        }
        if (rb2d.velocity.x < -maxspeed)
        {
            rb2d.velocity = new Vector2(-maxspeed, rb2d.velocity.y);
        }
        float wXVel = Input.GetAxis("Horizontal") * xVel;
        float impulse = Input.GetAxis("Vertical") * jumpImpulse;

        rb2d.velocity = new Vector2(wXVel, rb2d.velocity.y);
        rb2d.AddForce(new Vector2(0, impulse), ForceMode2D.Impulse);
    }
}
