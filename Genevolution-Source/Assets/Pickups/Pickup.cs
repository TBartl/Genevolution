using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PickupType {
	feet, claw, wing, eye
}

public class Pickup : MonoBehaviour {
	public PickupType type;
	public GameObject pickupTextPrefab;

	bool alreadyGrabbed = false;

	[TextArea(3, 10)]
	public string displayText;

	void Start() {
		StartCoroutine(Wave());
	}

	public void OnPickup() {
		GameObject textCanvas = Instantiate(pickupTextPrefab);
		textCanvas.transform.position = this.transform.position;
		textCanvas.GetComponentInChildren<Text>().text = displayText;
		alreadyGrabbed = true;
		Destroy(this.gameObject);
	}

	IEnumerator Wave() {
		Vector3 originalPosition = this.transform.position;
		float offset = 0;
		float floatAmount = .2f;
		while (true) {
			this.transform.position = originalPosition + floatAmount * Vector3.up *(.5f + .5f * Mathf.Sin(offset));
			offset += Time.deltaTime * 3;
			yield return null;
		}
	}

	public bool Available() {
		return !alreadyGrabbed;
	}
}
