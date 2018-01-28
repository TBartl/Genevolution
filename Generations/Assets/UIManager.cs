using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	CanvasGroup group;

	public Text timer;
	public Image fill;

	float alpha;
	public float alphaSpeed = 5f;

	void Awake() {
		group = this.GetComponent<CanvasGroup>();
	}

	void Update() {
		if (Nest.currentPlayer) {
			PlayerLife life = Nest.currentPlayer.GetComponent<PlayerLife>();
			fill.fillAmount = life.lifeSpan / life.maxLifeSpan;
			timer.text = Mathf.CeilToInt(life.lifeSpan).ToString();
			alpha += alphaSpeed * Time.deltaTime;
		}
		else {
			fill.fillAmount = 0;
			timer.text = "";
			alpha -= alphaSpeed * Time.deltaTime;
		}
		alpha = Mathf.Clamp01(alpha);
		group.alpha = alpha;
	}
}
