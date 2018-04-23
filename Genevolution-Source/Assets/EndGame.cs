using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {

	[TextArea]
	public string text;

	public Text box;

	public float charTime = .05f;

	void OnTriggerEnter2D(Collider2D collision) {
		Nest.currentlyActiveNest = null;
		collision.GetComponent<PlayerLife>().Kill();
		StartCoroutine(ShowText());
	}

	IEnumerator ShowText() {
		for (int i = 0; i < text.Length - 1; i++) {
			box.text = text.Substring(0, i) + "<color=#00000000>" + text.Substring(i + 1) + "</color>";
			yield return new WaitForSeconds(charTime);
		}
		yield return new WaitForSeconds(5);
		SceneManager.LoadScene(0);
	}
}
