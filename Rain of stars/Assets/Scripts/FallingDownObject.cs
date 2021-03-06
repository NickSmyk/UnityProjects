using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDownObject : MonoBehaviour {

	public float speed;
	private Rigidbody2D rb;
	private Vector2 screenBounds;
	private float xCoordinate;

	// Start is called before the first frame update
	void Start() {
		speed = speed == 0 ? 2.0f : speed;
		screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
		rb = this.GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(0, -speed);

	}



	private void OnBecameInvisible() {
		Destroy(this.gameObject);

	}

	private void OnTriggerEnter2D(Collider2D collision) {
		GameControlScript.OnGettingDamage();
		if (collision.tag == "Player" && !GameControlScript.isInvulnerable) {
			GameControlScript.lastTimeBeingHit = DateTime.Now;
			if (GameControlScript.health != 1) {
				GameControlScript.PlayInvulnerabilityAnimation();
				FindObjectOfType<AudioManager>().Play("SnailIsDamaged");
			}
			GameControlScript.health -= 1;
		}
	}
}
