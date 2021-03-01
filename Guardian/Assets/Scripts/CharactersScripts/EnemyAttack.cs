using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
	private float TimeBetweenAttack;
	public float StartTimeBetweenAttack;

	public Transform AttackPosition;
	public LayerMask CoreLayerMask;
	public LayerMask WhatAreEnemies;
	public float AttackRange;
	Animator Animator;
	private EnemyScript Enemy;
	void Start() {
		Animator = GetComponent<Animator>();
		Enemy = gameObject.GetComponent<EnemyScript>();
	}

	// Update is called once per frame
	void Update() {
		if (Enemy.IsEnemyDead())
			return;
		Collider2D[] enemiesToAttack = Physics2D.OverlapCircleAll(AttackPosition.position, AttackRange, WhatAreEnemies);
		Collider2D[] coreToAttack = Physics2D.OverlapCircleAll(AttackPosition.position, AttackRange, CoreLayerMask);
		Collider2D core = coreToAttack.Length == 0? null : coreToAttack[0];
		Character characterToAttack = enemiesToAttack.Length > 0 ? enemiesToAttack[0].gameObject.GetComponent<Character>() : null;
		if ((enemiesToAttack.Length > 0 && characterToAttack.CanCharacterBeAttacked()) || core != null)
			Enemy.SetAttackState(true);
		if (TimeBetweenAttack <= 0) {
			if ((enemiesToAttack.Length > 0 &&characterToAttack.CanCharacterBeAttacked()) || core != null ) {
				//gameObject.GetComponent<EnemyScript>().Stop();
				//FindObjectOfType<AudioManager>().Play("DeagonWarriorHit");
				Animator.SetTrigger("attack");
				TimeBetweenAttack = StartTimeBetweenAttack;
			} else
				Enemy.SetAttackState(false);

		} else {
			TimeBetweenAttack -= Time.deltaTime;
		}

	}
	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(AttackPosition.position, AttackRange);
	}

	public void Attack() {

	}

	public void DealDamage() {
		Collider2D[] enemiesToAttack = Physics2D.OverlapCircleAll(AttackPosition.position, AttackRange, WhatAreEnemies);
		Collider2D[] coresToAttack = Physics2D.OverlapCircleAll(AttackPosition.position, AttackRange, CoreLayerMask);
		float damage = Enemy.GetCurrentDamage();
		foreach (var character in enemiesToAttack) {
			if (!character.GetComponent<Character>().CanCharacterBeAttacked())
				continue;
			character.GetComponent<Character>().TakeDamage(damage);
		}
		foreach(var core in coresToAttack) {
			core.GetComponent<Core>().TakeDamage(damage);
		}
	}
	public void OnEndAttack() {

	}

	public void OnFinishAttack() {
		Animator.SetTrigger("stationary");
	}





}
