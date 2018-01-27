using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

	PlayerMovement movement;

	void Awake() {
		movement = this.GetComponent<PlayerMovement>();
	}

	// Update is called once per frame
	void Update () {
		Vector2 velocity = movement.GetVelocity();

		if (velocity.x > 0) {
			this.transform.localScale = new Vector3(-1, 1, 1);
		} else {
			this.transform.localScale = new Vector3(1, 1, 1);
		}
		
	}
}
