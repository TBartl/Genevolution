using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
	public float amount;

	Transform camTransform;

	Vector3 originalPos;
	Vector3 originalCamPos;

	// Use this for initialization
	void Start () {
		originalPos = this.transform.position;

		camTransform = Camera.main.transform;
		originalCamPos = camTransform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 offset = camTransform.position - originalCamPos;
		this.transform.position = originalPos + offset * amount;		
	}
}
