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
		Character character = collision.GetComponent<Character>();
		if (character == null)
			return;
		if (character.IsCharacterAtMaximumLife()) {
			character.IncreaseHPMultiplier(2);
			GameControl.Notify(EnumMethods.GetDescription(Notifications.IncreasedMaximumLife));
		}
		else {
			character.Heal(5);
			GameControl.Notify(EnumMethods.GetDescription(Notifications.Healing));
		}
		Destroy(transform.gameObject);
	}
}
