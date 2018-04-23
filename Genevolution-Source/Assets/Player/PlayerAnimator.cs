using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

	PlayerMovement movement;

	void Awake() {
		movement = this.GetComponent<PlayerMovement>();
	}

	// Update is called once per frame
	void Update() {
		Vector2 velocity = movement.GetVelocity();

		if (Mathf.Abs(velocity.x) > .05f) {
			if (velocity.x > 0) {
				this.transform.localScale = new Vector3(-1, 1, 1);
			}
			else {
				this.transform.localScale = new Vector3(1, 1, 1);
			}
		}

		foreach (Animator anim in this.GetComponentsInChildren<Animator>()) {
			anim.SetFloat("HorizontalSpeed", Mathf.Abs(movement.GetVelocity().x) / movement.maxHorizontalSpeed);
			anim.SetBool("Airborne", !movement.GetGrounded());
			anim.SetFloat("VerticalSpeed", Mathf.Abs(movement.GetVelocity().y));
		}
	}
}
