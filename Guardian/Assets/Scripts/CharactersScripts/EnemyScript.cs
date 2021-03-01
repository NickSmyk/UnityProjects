using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
	private Rigidbody2D rb;
	private Animator anim;
	private float moveSpeed = 0.5f;
	private float dirX;
	private bool facingRight = false;
	private Vector3 localScale;

	private float EnemyBaseHealth = 100f;
	private float HealthMultiplier = 1;
	private float CurrentHealth;

	private float BaseDamage = 25f;
	private float BaseDamageMultiplier = 0f;
	private float DamageMultiplier = 1f;
	private float CurrentDamage;

	private bool CanAttack = false;
	private bool IsDead = false;
	private float ExperienceOnDeath = 100f;
	// Start is called before the first frame update
	void Start() {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		localScale = transform.localScale;

		HealthMultiplier = 1f;
		DamageMultiplier = 1f;
		CurrentHealth = EnemyBaseHealth * HealthMultiplier;
		CurrentDamage = (BaseDamage + BaseDamage * BaseDamageMultiplier) * DamageMultiplier;


		dirX = -1f;
	}

	// Update is called once per frame
	void Update() {
		if (CanAttack || IsDead)
			dirX = 0f;
		else
			dirX = -1f;
		//dirX = CrossPlatformInputManager.GetAxisRaw("Horizontal") * moveSpeed;
		transform.position += new Vector3(dirX, 0, 0) * Time.deltaTime * moveSpeed;


		/*if (CrossPlatformInputManager.GetButtonDown("Jump"))
			rb.AddForce(Vector2.up * 300f);*/

		//anim.SetBool("isRunning", false);
		if (Mathf.Abs(dirX) > 0) {
			anim.SetBool("isRunning", true);
			//FindObjectOfType<AudioManager>().Stop("DeagonWarriorHit");
		} else {
			anim.SetBool("isRunning", false);
		}


	}

	public void PlayAttackSound() {
		FindObjectOfType<AudioManager>().Play("DeagonWarriorHit");
	}
	private void FixedUpdate() {
		rb.velocity = new Vector2(dirX, rb.velocity.y);
	}
	private void LateUpdate() {
		if (Time.timeScale == 0f) {
			return;
		}
		if (dirX > 0)
			facingRight = true;
		else if (dirX < 0)
			facingRight = false;
		if ((facingRight && localScale.x < 0) || (!facingRight && localScale.x > 0))
			localScale.x *= -1;

		transform.localScale = localScale;
	}

	public void TakeDamage(float damage) {
		CurrentHealth -= damage;
		if (CurrentHealth <= 0 && !IsDead) {
			Die();
			GameControl.IncreaseScore(1);
			GameControl.GrantExperience(ExperienceOnDeath);
			Debug.Log("Enemy has been slained!");
			return;
		}
		Debug.Log("Enemy was damaged!");
	}
	public void Die() {
		IsDead = true;
		anim.SetBool("isRunning", false);
		FindObjectOfType<AudioManager>().Stop("DeagonWarriorHit");
		anim.SetTrigger("death");
	}

	public void OnDeath() {
		GameControl.DropAnItem(transform);
		Destroy();
	}
	public void Destroy() {
		Destroy(gameObject);
	}
	public float GetCurrentHealth() {
		return CurrentHealth;
	}
	public float GetCurrentDamage() {
		return CurrentDamage;
	}

	public void Stop() {
		dirX = 0f;
	}
	public void Move() {
		dirX = -1f;
	}
	public void SetAttackState(bool state) {
		CanAttack = state;
	}
	public bool IsEnemyDead() {
		return IsDead;
	}
}
