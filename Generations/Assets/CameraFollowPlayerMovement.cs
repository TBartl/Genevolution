using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayerMovement : MonoBehaviour {

	PlayerMovement playerMovement;

	public float power = 3f;
	public float verticalOffset = 5f;
	public float forceOffset = 10f;

	// Use this for initialization
	void Start () {
		playerMovement = FindObjectOfType<PlayerMovement>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		float lastGroundHeight = playerMovement.GetLastGroundPosition().y;
		Vector3 targetPosition = new Vector3(
			playerMovement.transform.position.x,
			lastGroundHeight + verticalOffset,
			-10);

		if (Mathf.Abs(playerMovement.transform.position.y - lastGroundHeight) >= forceOffset) {
			targetPosition.y = playerMovement.transform.position.y;
		}

		this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * power);
	}
}
