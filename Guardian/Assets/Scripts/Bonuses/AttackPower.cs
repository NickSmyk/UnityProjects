﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPower : MonoBehaviour {
	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "Ground") {
			transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
			return;
		}
		if (collision.tag != "Player")
			return;
		collision.GetComponent<Character>().IncreaseAttackDamageMultiplier(1);
		Destroy(transform.gameObject);
		GameControl.Notify(EnumMethods.GetDescription(Notifications.IncreasedAttackDamage));
	}
}
