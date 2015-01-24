using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;



public class SimplePlayer : MonoBehaviour {

	public AudioClip jumpSound;
	public AudioClip deathSound;

	public float speed = 0;
	//public float maxSpeed = 10;
	public float acceleration = .01f;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;

	float maxFallSpeed = 0;

	private float jumpHold;
	private bool jumpTrigger = false;
	
	public bool grounded;
	public bool isUpright = true;


	Transform topRight;
	Transform topLeft;
	Transform bottomLeft;
	Transform bottomRight;

	Transform sideTopRight;
	Transform sideBottomRight;

	public GameObject explosion;

	Animator anim;
	GameObject sprite;
	
	void Start(){

		maxFallSpeed = float.MaxValue;


		topRight = transform.Find("top_right").transform;
		topLeft = transform.Find("top_right").transform;
		bottomLeft = transform.Find("bottom_left").transform;
		bottomRight = transform.Find("bottom_right").transform;

		sideTopRight = transform.Find("side_top_right").transform;
		sideBottomRight = transform.Find("side_bottom_right").transform;

		anim = GetComponentInChildren<Animator>();
		sprite = anim.gameObject;
	}
	

	public Vector3 GetState(){
		return moveDirection;
	}

	Vector3 GetInput() {
		Vector3 move = moveDirection;

		move.x = Input.GetAxis ("Horizontal") * Time.fixedDeltaTime * speed * 100;
		//if (Input.GetAxis ("Horizontal1") != 0 && Input.GetAxis ("Horizontal2") != 0) {
						//move.x = (Input.GetAxis ("Horizontal1") + Input.GetAxis ("Horizontal2")) * Time.fixedDeltaTime * speed * 50;
				//}

			if (jumpHold > 0) {
			jumpHold -= Time.fixedDeltaTime * 200;		
		}



		if (grounded) {

			//can only be moving away from ground, not into it
			if(isUpright){
				move.y = Mathf.Max(move.y, 0f);
			}
			else{
				move.y = Mathf.Min(move.y, 0f);
			}
		}

		else {
			if (isUpright){
				move.y -= (gravity - jumpHold) * Time.fixedDeltaTime;
			}
			else{
				move.y += (gravity - jumpHold)* Time.fixedDeltaTime;

			}

			move.y = Mathf.Clamp(move.y, -maxFallSpeed, maxFallSpeed);

		}

		if (jumpTrigger) {
			jumpTrigger = false;
			//audio.pitch = (jumpSpeed / maxJump);
			audio.PlayOneShot (jumpSound);
			//rigidbody2D.AddForce (Vector3.up * jumpSpeed * 100);
			if (isUpright){
			move.y = jumpSpeed;
			}
			else
				move.y = -jumpSpeed;
			jumpHold = gravity;

		}

		//ReplayManager.Instance.SendMessage ("RecordInput", move);
//		Debug.Log (move);

		return move;

	}

	void Update(){
		CheckUpright ();
		CheckGrounded ();

		if (grounded && (Input.GetButtonDown ("Jump") || Input.GetMouseButtonDown(0))) {
			jumpTrigger = true;			
		}

		if (jumpHold > 0 && (Input.GetButtonUp ("Jump") || Input.GetMouseButtonUp(0))) {
			jumpHold = 0;		
		}
	}

	void FixedUpdate(){
		ApplyMove (GetInput ());

	}

	void Jump(){

	}

	void CheckGrounded(){
				bool g = false;
				int groundOnly = 1 << LayerMask.NameToLayer ("Ground");


				//if above center line
				if (isUpright) {
						if (moveDirection.y > 0) {
								g = false;
						} else {
								g = ((Physics2D.Linecast (bottomLeft.position + Vector3.up, bottomLeft.position, groundOnly) 
										|| Physics2D.Linecast (bottomRight.position + Vector3.up, bottomRight.position, groundOnly))
			);
						}
				//if below center line
				} else {
						if (moveDirection.y < 0) {
								g = false;
						} else {
								g = ((Physics2D.Linecast (topLeft.position + Vector3.down, topLeft.position, groundOnly) 
										|| Physics2D.Linecast (topRight.position + Vector3.down, topRight.position, groundOnly))
			     );
						}

				
				
				}

		if (grounded == true && g == false) {
						anim.SetBool ("jumping", true);
			maxFallSpeed = float.MaxValue;
				} else if (grounded == false && g == true) {
			anim.SetBool ("jumping", false);

				}

		grounded = g;
	}

	bool isBlockedRight(){

		int groundOnly = 1 << LayerMask.NameToLayer ("Ground");
		
		bool b = ((Physics2D.Linecast (sideBottomRight.position + Vector3.left, sideBottomRight.position, groundOnly) 
		           || Physics2D.Linecast (sideTopRight.position + Vector3.left, sideTopRight.position, groundOnly))
		          //&& rigidbody.velocity.y <= 0);
		          );
		//Debug.Log (b);
		return b;
		}

	void ApplyMove(Vector3 move){
		moveDirection = move;
		rigidbody2D.velocity = move;
		//Debug.Log (move + " " + grounded);

	}

	void CheckUpright(){
		if (!isUpright && this.transform.position.y >= 0) {
						isUpright = true;
			FlipSprite();
			maxFallSpeed = Mathf.Abs(moveDirection.y);
				} else if (isUpright && this.transform.position.y < 0) {
			isUpright = false;
			FlipSprite();
			maxFallSpeed = Mathf.Abs(moveDirection.y);
		}
	}

	void FlipSprite(){
		Vector3 newScale = sprite.transform.localScale;
		newScale.y = -newScale.y;
		sprite.transform.localScale = newScale;
		
		Vector3 newPos = sprite.transform.localPosition;
		newPos.y = -newPos.y;
		sprite.transform.localPosition = newPos;
	}

	void Die(){
		GameObject.Instantiate (explosion, this.transform.position, Quaternion.identity);
		this.transform.position = new Vector3 (-14,12,0);
		audio.PlayOneShot (deathSound);
	}


	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Harmful")
						Die ();		
	}

	void OnTriggerExit2D(Collider2D other)
	{

	}


		void OnDrawGizmos() {
			Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube (this.transform.position, this.transform.localScale);
		}


}
