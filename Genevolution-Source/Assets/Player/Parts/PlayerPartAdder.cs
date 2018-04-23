using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartData {
	public GameObject prefab;
	public List<GameObject> instances;
}

[System.Serializable]
public struct PartOffset {
	public Vector3 posOffset;
	public int layerOffset;
}

public class PlayerPartAdder : MonoBehaviour {

	PlayerStats stats;

	public PartData feet;
	public PartData claw;
	public PartData wing;
	public PartData eye;

	public Transform body;

	public List<PartOffset> offsets;

	void Awake() {
		stats = this.GetComponent<PlayerStats>();
	}

	void Update() {
		CompareAndUpdate(PlayerStats.jumpPower, feet);
		CompareAndUpdate(PlayerStats.wallJumps, claw);
		CompareAndUpdate(PlayerStats.airJumps, wing);
		CompareAndUpdate(PlayerStats.vision, eye);
	}

	void CompareAndUpdate(int realAmount, PartData partData) {
		while (partData.instances.Count < realAmount) {
			GameObject newPart = Instantiate(partData.prefab, body);
			PartOffset offset = offsets[Mathf.Min(offsets.Count - 1, partData.instances.Count)];
			newPart.transform.localPosition = offset.posOffset * 3;
			newPart.transform.localRotation = Quaternion.identity;

			SpriteRenderer newPartSR = newPart.GetComponentInChildren<SpriteRenderer>();
			newPartSR.sortingOrder = newPartSR.sortingOrder + offset.layerOffset;

			partData.instances.Add(newPart);
		}
	}
}
