using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollowPlayerMovement : MonoBehaviour {

	Camera cam;
	PlayerMovement playerMovement;
	PlayerStats stats;

	public float power = 3f;
	public float lookAheadAmount = 1.5f;
	public float verticalOffset = 5f;
	public float forceOffset = 10f;

	public AnimationCurve sizeByStat;

	Bounds levelBounds;

	Vector3 targetPosition;

	float playerAliveTime;

	// Use this for initialization
	void Start () {
		cam = this.GetComponent<Camera>();
		levelBounds = FindObjectOfType<Tilemap>().localBounds;
		targetPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (Nest.currentPlayer != null && playerMovement == null) {
			playerMovement = Nest.currentPlayer.GetComponent<PlayerMovement>();
			stats = Nest.currentPlayer.GetComponent<PlayerStats>();
		}

		if (playerMovement) {
			float lastGroundHeight = playerMovement.GetLastGroundPosition().y;
			targetPosition = new Vector3(
				playerMovement.transform.position.x + lookAheadAmount * playerMovement.GetVelocity().x / playerMovement.maxHorizontalSpeed,
				lastGroundHeight + verticalOffset,
				-10);

			if (Mathf.Abs(playerMovement.transform.position.y - lastGroundHeight) >= forceOffset) {
				targetPosition.y = playerMovement.transform.position.y;
			}
			cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, sizeByStat.Evaluate(PlayerStats.vision), Time.deltaTime);
			playerAliveTime = .5f;
		} else if (playerAliveTime <= 0) {
			targetPosition = Nest.currentlyActiveNest.transform.position + Vector3.back * 10;
		}

		playerAliveTime -= Time.deltaTime;

		Bounds orthoBounds = OrthoBounds(cam);
		targetPosition.x = Mathf.Clamp(
			targetPosition.x,
			levelBounds.center.x - .5f * levelBounds.size.x + .5f * orthoBounds.size.x + .5f,
			levelBounds.center.x + .5f * levelBounds.size.x - .5f * orthoBounds.size.x - .5f);
		targetPosition.y = Mathf.Clamp(
			targetPosition.y,
			levelBounds.center.y - .5f * levelBounds.size.y + .5f * orthoBounds.size.y + 1,
			levelBounds.center.y + .5f * levelBounds.size.y - .5f * orthoBounds.size.y);

		this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * power);
	}

	Bounds OrthoBounds(Camera camera) {
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = camera.orthographicSize * 2;
		Bounds bounds = new Bounds(
			camera.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
		return bounds;
	}
}
