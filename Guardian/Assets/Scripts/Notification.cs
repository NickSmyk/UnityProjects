using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour {
	Text Text;
	private float FadeDuration = 2f;
	private float Speed = 100f;

	// Start is called before the first frame update
	void Start() {
		Text = this.GetComponent<Text>();
		StartCoroutine(Fade());
	}

	// Update is called once per frame
	void Update() {
		this.transform.Translate(Vector3.up * Time.deltaTime * Speed);
	}

	public IEnumerator Fade() {
		float fadeSpeed = (float)1.0 / FadeDuration;
		Color color = Text.color;
		for (float t = 0.0f; t < 1f; t += Time.deltaTime * fadeSpeed) {
			color.a = Mathf.Lerp(1, 0, t);
			Text.color = color;
			yield return true;
		}
		Destroy(this.gameObject);
	}
}
