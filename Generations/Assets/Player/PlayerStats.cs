using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
	public int jumpPower = 0;
	public int wallJumps = 0;
	public int airJumps = 0;
	public int vision = 0;

	void OnTriggerEnter2D(Collider2D collision) {
		Pickup pickup = collision.GetComponent<Pickup>();
		if (pickup && pickup.Available()) {
			if (pickup.type == PickupType.feet)
				jumpPower += 1;
			if (pickup.type == PickupType.claw)
				wallJumps += 1;
			if (pickup.type == PickupType.wing)
				airJumps += 1;
			if (pickup.type == PickupType.eye)
				vision += 1;
			pickup.OnPickup();
		}
	}
}
