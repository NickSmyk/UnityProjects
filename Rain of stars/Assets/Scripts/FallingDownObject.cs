using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDownObject : MonoBehaviour {
	/*private float startPosX;
	private float startPosY;
	private bool isBeingHeld = false;*/


	public float speed;
	private Rigidbody2D rb;
	private Vector2 screenBounds;
	private float xCoordinate;

	// Start is called before the first frame update
	void Start() {
		speed = speed == 0? 2.0f : speed;
		screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
		rb = this.GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(0/*Random.Range(-1, 1)*/, -speed);
		//rb.MovePosition((Vector2)transform.position + new Vector2(Random.Range(-3, 3), -speed));

	}

	// Update is called once per frame
	/*void Update() {
		if (isBeingHeld == true) {
			Vector3 mousePos;
			mousePos = Input.mousePosition;
			mousePos = Camera.main.ScreenToWorldPoint(mousePos);
			this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);
		}
	}
	private void OnMouseDown() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mousePos;
			mousePos = Input.mousePosition;
			mousePos = Camera.main.ScreenToWorldPoint(mousePos);

			startPosX = mousePos.x - this.transform.localPosition.x;
			startPosX = mousePos.y - this.transform.localPosition.y;

			isBeingHeld = true;
		}
	}*/

	/*private void OnMouseUp() {
		rb.velocity = new Vector2(0, 0);
		rb.gravityScale = 1;
		isBeingHeld = false;
	}*/

	private void OnBecameInvisible() {
		Destroy(this.gameObject);

	}

	private void OnTriggerEnter2D(Collider2D collision) {
		GameControlScript.OnGettingDamage();
		if (collision.tag == "Player" && !GameControlScript.isInvulnerable) {
			GameControlScript.lastTimeBeingHit = DateTime.Now;
			if(GameControlScript.health != 1) {
				GameControlScript.PlayInvulnerabilityAnimation();
				FindObjectOfType<AudioManager>().Play("SnailIsDamaged");
			}
			GameControlScript.health -= 1;
		}
	}
}
