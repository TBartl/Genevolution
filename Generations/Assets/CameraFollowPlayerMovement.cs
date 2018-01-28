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

	// Use this for initialization
	void Start () {
		cam = this.GetComponent<Camera>();
		playerMovement = FindObjectOfType<PlayerMovement>();
		stats = playerMovement.GetComponent<PlayerStats>();
		levelBounds = FindObjectOfType<Tilemap>().localBounds;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!playerMovement)
			return;

		float lastGroundHeight = playerMovement.GetLastGroundPosition().y;
		Vector3 targetPosition = new Vector3(
			playerMovement.transform.position.x + lookAheadAmount * playerMovement.GetVelocity().x / playerMovement.maxHorizontalSpeed,
			lastGroundHeight + verticalOffset,
			-10);

		if (Mathf.Abs(playerMovement.transform.position.y - lastGroundHeight) >= forceOffset) {
			targetPosition.y = playerMovement.transform.position.y;
		}
		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, sizeByStat.Evaluate(stats.vision), Time.deltaTime);


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
