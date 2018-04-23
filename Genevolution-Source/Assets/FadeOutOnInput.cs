using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutOnInput : MonoBehaviour {
	public List<MonoBehaviour> components;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Jump")) {
			foreach (MonoBehaviour c in components) {
				c.enabled = true;
			}
		}
	}
}
