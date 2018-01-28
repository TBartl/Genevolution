using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour {

	public float maxLifeSpan = 10.9f;
	public float lifeSpan;

	public GameObject skullPrefab;

	void Start() {
		lifeSpan = maxLifeSpan;
		
	}

	void Update() {
		float originalLifespan = lifeSpan;
		lifeSpan -= Time.deltaTime;
		if (lifeSpan < 3 && lifeSpan > 0 && Mathf.CeilToInt(lifeSpan) != Mathf.CeilToInt(originalLifespan)) {
			float volume = 1 - (Mathf.Ceil(lifeSpan) - 1) / 3;
			SoundManager.S.tick.volume = volume * .8f;
			SoundManager.S.tick.Play();
		}
		if (lifeSpan < 0) {
			SoundManager.S.die.Play();
			Kill();
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.layer == LayerMask.NameToLayer("Spikes")) {
			SoundManager.S.die.Play();
			Kill();
		}
	}

	public void Kill() {
		Destroy(this.gameObject);
		GameObject skull = Instantiate(skullPrefab, this.transform.position, Quaternion.identity);
		skull.GetComponent<Rigidbody2D>().AddTorque(-this.GetComponent<PlayerMovement>().GetVelocity().x * 8);
		skull.GetComponent<Rigidbody2D>().velocity = this.GetComponent<PlayerMovement>().GetVelocity();
		skull.transform.localScale = this.transform.localScale;
	}

}
