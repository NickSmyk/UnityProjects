using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour {
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Ground") {
			transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
			return;
		}
		if (collision.tag != "Player")
			return;
		collision.GetComponent<Character>().Heal(5);
		Destroy(transform.gameObject);
		GameControl.Notify(EnumMethods.GetDescription(Notifications.Healing));
	}
}
