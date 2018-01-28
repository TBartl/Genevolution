using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiseAndDisappear : MonoBehaviour {

	public float riseTime = 2f;
	public float riseAmount = 1f;

	Text text;

	IEnumerator Start() {
		text = this.GetComponentInChildren<Text>();
		Vector3 originalPosition = this.transform.position;
		for (float t = 0; t < riseTime; t += Time.deltaTime) {
			float p = t / riseTime;
			this.transform.position = originalPosition + Vector3.up * p;
			text.color = new Color(1f, 1f, 1f, 1 - p);
			yield return null;
		}
		Destroy(this.gameObject);
	}
}
