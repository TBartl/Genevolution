using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour {

	public GameObject female;

	public GameObject playerPrefab;

	public bool startAsCurrentlyActive = false;

	public static Nest currentlyActiveNest;
	public static GameObject currentPlayer;

	[HideInInspector]
	public bool spawning = false;

	public GameObject eggWhole;
	public GameObject eggBreak;

	public SpriteRenderer aDisplay;
	float aOpacityMultiplier = 0;

	public GameObject heartPrefab;

	bool previouslyNested = false;

	static bool firstSpawn = true;

	// Use this for initialization
	void Start () {
		if (startAsCurrentlyActive) {
			PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
			if (playerMovement)
				currentPlayer = playerMovement.gameObject;

			Activate();
			PlayerStats.jumpPower = 0;
			PlayerStats.vision = 1;
			PlayerStats.wallJumps = 0;
			PlayerStats.airJumps = 0;
		} else {
			StartCoroutine(DeactivateMate());
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (currentlyActiveNest == this && currentPlayer == null && !spawning) {
			StartCoroutine(SpawnPlayer());
		}

		if (spawning)
			aOpacityMultiplier += Time.deltaTime;
		else
			aOpacityMultiplier -= Time.deltaTime;
		aOpacityMultiplier = Mathf.Clamp01(aOpacityMultiplier);
		aDisplay.color = new Color(1f, 1f, 1f, aOpacityMultiplier * (.5f + .5f * Mathf.Sin(Time.time * 5)));
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (this == currentlyActiveNest)
			return;
		if (currentPlayer)
			currentPlayer.GetComponent<PlayerLife>().Kill();
		Activate();
		Instantiate(heartPrefab, this.transform.position, Quaternion.identity);
	}

	public void Activate() {
		if (currentlyActiveNest)
			currentlyActiveNest.Deactivate();
		currentlyActiveNest = this;
		StartCoroutine(ActivateMate());

		if (!previouslyNested) {
			previouslyNested = true;
			SoundManager.S.nestNew.Play();
		} else {
			SoundManager.S.nestVisited.Play();
		}
	}

	public void Deactivate() {
		if (this != currentlyActiveNest)
			return;
		currentlyActiveNest = null;
		StartCoroutine(DeactivateMate());
	}

	IEnumerator ActivateMate() {
		float activateTime = .20f;
		for (float t = 0; t < activateTime; t += Time.deltaTime) {
			float p = t / activateTime;
			Color c = Color.Lerp(Color.gray, Color.white, p);
			SetMateColor(c);
			yield return null;
		}
		SetMateColor(Color.white);
	}

	IEnumerator DeactivateMate() {
		float deactivateTime = .20f;
		for (float t = 0; t < deactivateTime; t += Time.deltaTime) {
			float p = t / deactivateTime;
			Color c = Color.Lerp(Color.white, Color.gray, p);
			yield return null;
		}
		SetMateColor(Color.gray);

	}

	void SetMateColor(Color c) {
		foreach (SpriteRenderer sr in female.GetComponentsInChildren<SpriteRenderer>()) {
			sr.color = c;
		}
	}

	IEnumerator	SpawnPlayer() {
		spawning = true;
		GameObject egg = Instantiate(eggWhole, this.transform.position - Vector3.up * .25f, Quaternion.identity);
		while (!Input.GetButton("Jump"))
			yield return null;
		Destroy(egg);
		egg = Instantiate(eggBreak, this.transform.position - Vector3.up * .25f, Quaternion.identity);
		yield return new WaitForSeconds(.25f);
		SoundManager.S.eggCrack.Play();
		yield return new WaitForSeconds(.25f);
		spawning = false;
		currentPlayer = Instantiate(playerPrefab, this.transform.position, Quaternion.identity);

		if (firstSpawn) {
			SoundManager.S.music.Play();
			firstSpawn = false;
		}
	}
}
