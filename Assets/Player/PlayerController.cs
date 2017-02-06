using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Collider2D))]
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(Player))]
public class PlayerController : NetworkBehaviour {
	[SerializeField] private float xVel = 10f;
	[SerializeField] private float yVel = 10f;
    [SerializeField] private float maxspeed = 10f;

    //used for jumping animations and flipping
    public bool grounded;
    public bool isLeft;
	[SyncVar (hook="OnChangeFacingRight")]
    public bool facingRight = true;

    //private references
    private Rigidbody2D rb2d;
	private Collider2D coll;
    private Animator anim;
	private Player player;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
		coll = GetComponent<Collider2D> ();
		player = GetComponent<Player> ();
    }
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) return;
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));

        //flips player
        if (Input.GetAxis("Horizontal") > 0)
        {
			SetFacingRight (true);
        }
        else if (Input.GetAxis("Horizontal") < 0) 
        {
			SetFacingRight (false);
        }

		// check if grounded
		float percentXSize = 0.5f;
		Vector2 castOrigin = new Vector2 (coll.bounds.min.x + (1.0f - percentXSize) / 2.0f * coll.bounds.size.x, coll.bounds.min.y); // same shape as collider
		Vector2 castSize = new Vector2 (coll.bounds.size.x * percentXSize, 0.01f); // almost same size as collider
		Vector2 castDir = new Vector2 (0f, -1f); // vertical cast
		float maxGroundDist = 0.1f;
		RaycastHit2D hit = Physics2D.BoxCast (castOrigin, castSize, 0f, castDir, maxGroundDist, player.GroundLayerMask ());

		if (hit.collider != null) {
			grounded = true;
		} else {
			grounded = false;
		}
    }

	public void SetFacingRight(bool facingRight) {
		CmdSetFacingRight (facingRight);
		UpdateFacingRight (facingRight);
	}

	[Command]
	void CmdSetFacingRight(bool facingRight) {
		this.facingRight = facingRight;
	}

	void OnChangeFacingRight(bool facingRight) {
		if (!hasAuthority) {
			UpdateFacingRight (facingRight);
		}
	}

	void UpdateFacingRight(bool facingRight) {
		GetComponent<SpriteRenderer>().flipX = facingRight;

		this.facingRight = facingRight;
	}

    void FixedUpdate()
    {
		if (!isLocalPlayer) return;

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
		rb2d.velocity = new Vector2(wXVel, rb2d.velocity.y);
		
		if (grounded && Input.GetAxis("Vertical") > 0) {
			rb2d.velocity = new Vector2 (rb2d.velocity.x, yVel);
		}
    }
}
