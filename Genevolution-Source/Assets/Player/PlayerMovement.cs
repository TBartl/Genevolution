﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	Rigidbody2D rb;
	PlayerStats stats;

	public float maxHorizontalSpeed = 5f;
	public AnimationCurve accelerationByAmountNeeded;

	public float gravity = 13f;

	public float wallJumpHorizontalVelocity;

	public float letGoExtraGravity = 20f;

	public float maxFallSpeed = 15f;
	public float maxFallSpeedHuggingWall = 1f;

	public float maxJumpBuffer = .2f;
	public float maxRecentlyGroundedBuffer = .2f;
	public float maxRecentlyHugWallBuffer = .2f;

	public float maxFootstepInterval = .33f;
	float footstepInterval;

	Vector3 velocity;
	bool grounded = false;
	int lastGrounded = 0;
	bool justJumped = false;
	bool huggingWall = false;
	float lastWallSign;

	int remainingWallJumps;
	int remainingAirJumps;

	float jumpBuffer = 0;
	float recentlyGroundedBuffer = 0f;
	float recentlyHugWallBuffer = 0f;

	Vector2 lastGroundPosition;

	void Awake() {
		rb = this.GetComponent<Rigidbody2D>();
		stats = this.GetComponent<PlayerStats>();
		lastGroundPosition = this.transform.position;
	}

	void Update() {
		if (Input.GetButtonDown("Jump")) {
			jumpBuffer = maxJumpBuffer;
		}
		recentlyGroundedBuffer -= Time.deltaTime;
		jumpBuffer -= Time.deltaTime;
		recentlyHugWallBuffer -= Time.deltaTime;
	}

	void FixedUpdate() {
		justJumped = false;

		float targetSpeed = Input.GetAxis("Horizontal") * maxHorizontalSpeed;
		float diffHorizontal = targetSpeed - velocity.x;
		float changeHorizontal = Mathf.Min(
			Mathf.Abs(diffHorizontal),
			accelerationByAmountNeeded.Evaluate(Mathf.Abs(diffHorizontal)) * Time.deltaTime);
		velocity.x += Mathf.Sign(diffHorizontal) * changeHorizontal;
		
		velocity.y -= gravity * Time.deltaTime;

		velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);
		if (huggingWall && remainingWallJumps > 0)
			velocity.y = Mathf.Max(velocity.y, -maxFallSpeedHuggingWall);

		if (jumpBuffer > 0) {
			if (recentlyGroundedBuffer > 0) {
				Jump(PlayerStats.jumpPower + 0.5f);
				StartCoroutine(MonitorJumpArc());
				SoundManager.S.jump.Play();
			} else if (recentlyHugWallBuffer > 0 && remainingWallJumps > 0) {
				remainingWallJumps -= 1;
				velocity.x = lastWallSign * wallJumpHorizontalVelocity;
				Jump(1.5f);
				SoundManager.S.wallJump.Play();
			} else if (remainingAirJumps > 0) {
				remainingAirJumps -= 1;
				Jump(1.5f);
				SoundManager.S.airJump.Play();
			}
		}
		if (grounded)
			lastGrounded = 3;
		else
			lastGrounded -= 1;

		if (grounded)
			footstepInterval -= Time.deltaTime * Mathf.Abs(velocity.x) / maxHorizontalSpeed;
		if (footstepInterval < 0) {
			footstepInterval = maxFootstepInterval;
			SoundManager.S.footstep.Play();
		}


		grounded = false;
		huggingWall = false;
		rb.velocity = velocity;
	}

	void Jump (float jumpHeight) {
		velocity.y = 2 * jumpHeight / Mathf.Sqrt(2 * jumpHeight / gravity);
		jumpBuffer = 0;
		recentlyGroundedBuffer = 0;
		recentlyHugWallBuffer = 0;
		justJumped = true;
		grounded = false;
	}

	void OnCollisionStay2D(Collision2D collision) {
		foreach (ContactPoint2D contact in collision.contacts) {
			Vector2 normal = contact.normal;
			if (Vector2.Dot(Vector2.up, normal) > .9f && !justJumped) {
				velocity.y = 0;
				grounded = true;
				recentlyGroundedBuffer = maxRecentlyGroundedBuffer;
				lastGroundPosition = this.transform.position;
				remainingWallJumps = PlayerStats.wallJumps;
				remainingAirJumps = PlayerStats.airJumps;

				if (lastGrounded <= 0) {
					SoundManager.S.land.Play();
					footstepInterval = maxFootstepInterval;
				}
			}

			if (Mathf.Abs(normal.x) > .8f) {
				recentlyHugWallBuffer = maxRecentlyHugWallBuffer;
				huggingWall = true;
				lastWallSign = Mathf.Sign(normal.x);
			}
		}		
	}

	IEnumerator MonitorJumpArc() {
		while (!grounded && velocity.y > 0 && Input.GetButton("Jump")) {
			yield return null;
		}
		while (!grounded && velocity.y > 0) {
			velocity.y -= Mathf.Max(letGoExtraGravity * Time.deltaTime, Mathf.Abs(velocity.y));
			yield return null;
		}
	}


	public Vector2 GetVelocity() {
		return velocity;
	}
	public Vector2 GetLastGroundPosition() {
		return lastGroundPosition;
	}

	public bool GetGrounded() {
		return lastGrounded > 0;
	}

}
