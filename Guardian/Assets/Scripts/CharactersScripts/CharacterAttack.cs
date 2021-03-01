using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
	private float TimeBetweenAttack;
	private float StartTimeBetweenAttack;

	public Transform AttackPosition;
	public LayerMask WhatAreEnemies;
	public float AttackRange;
	Animator Animator;



    // Start is called before the first frame update
    void Start() {
		StartTimeBetweenAttack = 1f;
		Animator = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update() {
		Collider2D[] enemiesToAttack = Physics2D.OverlapCircleAll(AttackPosition.position, AttackRange, WhatAreEnemies);
		bool anyoneToAttack = enemiesToAttack.Length > 0 && IsAnyoneAlive(enemiesToAttack);
		if (!anyoneToAttack)
			return;
		if (TimeBetweenAttack <= 0 ) {
				Animator.SetTrigger("attack");
				FindObjectOfType<AudioManager>().Play("SwordSlash");
				TimeBetweenAttack = StartTimeBetweenAttack;
		} else {
			TimeBetweenAttack -= Time.deltaTime;
		}

	}
	public void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(AttackPosition.position, AttackRange);
	}

	public void Attack() {

	}

	public void DealDamage() {
		Collider2D[] enemiesToAttack = Physics2D.OverlapCircleAll(AttackPosition.position, AttackRange, WhatAreEnemies);
		float damage = gameObject.GetComponent<Character>().GetCharacterDamage();
		for (int i = 0; i < enemiesToAttack.Length; i++) {
			enemiesToAttack[i].GetComponent<EnemyScript>().TakeDamage(damage);
		}
	}

	public void OnFinishAttack() {
		Animator.SetTrigger("stationary");
	}


	List<Collider2D> GetAliveEnemies(Collider2D[] enemies) {
		List<Collider2D> aliveEnemies = new List<Collider2D>();
		foreach (var enemy in enemies) {
			if (enemy.gameObject.GetComponent<EnemyScript>().GetCurrentHealth() > 0)
				aliveEnemies.Add(enemy);
		}
		return aliveEnemies;
	}

	bool IsAnyoneAlive(Collider2D[] enemies) {
		foreach (var enemy in enemies) {
			if (enemy.gameObject.GetComponent<EnemyScript>().GetCurrentHealth() > 0)
				return true;
		}
		return false;
	}

	public bool IncreaseAttackSpeed(float multiplier) {
		if (StartTimeBetweenAttack == 0.25)
			return true;
		var aps = 1 / StartTimeBetweenAttack * (1 + multiplier / 100);
		StartTimeBetweenAttack = 1 / aps;
		if (StartTimeBetweenAttack < 0.25)
			StartTimeBetweenAttack = 0.25f;
		return true;
	}
}
