using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour {
	private float TimeBetweenAttack;
	private float StartTimeBetweenAttack;

	public Transform AttackPosition;
	public LayerMask WhatAreEnemies;
	public float AttackRange;
	Animator Animator;
	private Character MainCharacter;



	void Start() {
		StartTimeBetweenAttack = 1.5f;
		MainCharacter = gameObject.GetComponent<Character>();
		Animator = MainCharacter.GetComponent<Animator>();
	}

	void Update() {
		if (MainCharacter.GetCurrentHealth() <=0 || !MainCharacter.IsCharacterStationary() || MainCharacter.GetIsAttacking() || !MainCharacter.GetCanAttack())
			return;
		Collider2D[] enemiesToAttack = Physics2D.OverlapCircleAll(AttackPosition.position, AttackRange, WhatAreEnemies);
		bool anyoneToAttack = enemiesToAttack.Length > 0 && IsAnyoneAlive(enemiesToAttack);
		if (!anyoneToAttack || MainCharacter.GetIsRunning())
			return;
		if (TimeBetweenAttack <= 0) {
			MainCharacter.SetIsAttacking(true);
			MainCharacter.ChangeAnimationState(EnumMethods.GetDescription(MainCharacterAnimations.MainCharacterAttack));
			FindObjectOfType<AudioManager>().Play("SwordSlash");
			TimeBetweenAttack = StartTimeBetweenAttack;
			Invoke("OnFinishAttack", MainCharacter.GetTheLenghtOfCurrentAnimation());
		} else {
			TimeBetweenAttack -= Time.deltaTime;
		}

	}
	public void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(AttackPosition.position, AttackRange);
	}


	public void DealDamage() {
		Collider2D[] enemiesToAttack = Physics2D.OverlapCircleAll(AttackPosition.position, AttackRange, WhatAreEnemies);
		if (enemiesToAttack == null || enemiesToAttack.Length == 0)
			return;
		float damage = MainCharacter.GetCharacterDamage();
		for (int i = 0; i < enemiesToAttack.Length; i++) {
			enemiesToAttack[i].GetComponent<EnemyScript>().TakeDamage(damage);
		}
	}

	public void OnFinishAttack() {
		if (!MainCharacter.GetIsAttacking())
			return;
		MainCharacter.SetIsAttacking(false);
		MainCharacter.ChangeAnimationState(EnumMethods.GetDescription(MainCharacterAnimations.MainCharacterIdle));
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
		if (StartTimeBetweenAttack == 0.5) {
			GameControl.IsAttackSpeedMax = true;
			return true;
		}
		var aps = 1 / StartTimeBetweenAttack * (1 + multiplier / 100);
		StartTimeBetweenAttack = 1 / aps;
		if (StartTimeBetweenAttack < 0.5) {
			StartTimeBetweenAttack = 0.5f;
			GameControl.IsAttackSpeedMax = true;
		}
		return true;
	}
}
