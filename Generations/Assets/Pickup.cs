using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType {
	feet, claw, wing, eye
}

public class Pickup : MonoBehaviour {
	public PickupType type;
}
