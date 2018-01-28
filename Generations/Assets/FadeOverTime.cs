using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOverTime : MonoBehaviour {
	public float time;

	// Use this for initialization
	IEnumerator Start () {
		SpriteRenderer[] srs = this.GetComponentsInChildren<SpriteRenderer>();
		for (float t = 0; t < time; t += Time.deltaTime) {
			float p = t / time;
			foreach (SpriteRenderer sr in srs) {
				sr.color = new Color(1f, 1f, 1f, 1 - p);
			}
			yield return null;
		}
	}
}
