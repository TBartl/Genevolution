using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour {
	float timeUntilReset = 5000;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Reset")) {
			SceneManager.LoadScene(0);
		}
		timeUntilReset -= Time.deltaTime;
		if (Input.GetButton("Jump")) {
			timeUntilReset = 20;
		}
		if (timeUntilReset <= 0) {
			Nest.currentlyActiveNest = null;
			Nest.currentPlayer = null;
			PlayerStats.airJumps = 0;
			PlayerStats.jumpPower = 0;
			PlayerStats.vision = 1;
			PlayerStats.wallJumps = 0;
			SceneManager.LoadScene(0);
		}
	}
}
