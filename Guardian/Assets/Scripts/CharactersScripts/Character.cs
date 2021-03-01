using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	private Rigidbody2D rb;
	private Animator anim;
	private float moveSpeed;
	private float dirX;
	private bool facingRight = true;
	private Vector3 localScale;

	private float BaseDamage = 35f;
	private float BaseDamageMultiplier = 0f;
	private float DamageMultiplier = 1f;
	private float CurrentDamage;

	private float BaseHealth = 300f;
	private float HealthMultiplier = 1;
	private float CurrentHealth;
	public HealthBarScript HealthBar;

	private bool CanBeAttacked = false;
	#region Stats
	public LevelingMenu LevelingMenu;
	private int AttackSpeedStat = 0;
	private int AttackDamageStat = 0;
	private int HealthStat = 0;
	private int CurrentLevel = 1;
	private int Points = 1;
	private float TotalExperience = 0;
	private float ExpToLevelUp = 100;
	#endregion



	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		localScale = transform.localScale;

		AttackSpeedStat = 0;
		AttackDamageStat = 0;
		HealthStat = 0;
		CurrentLevel = 1;
		Points = 0;
		TotalExperience = 0;
		ExpToLevelUp = 100;

		DamageMultiplier = 1f;
		HealthMultiplier = 1f;
		CurrentHealth = BaseHealth * HealthMultiplier;
		CurrentDamage = (BaseDamage + BaseDamage * BaseDamageMultiplier) * DamageMultiplier;



		UpdateHealthBar();
		moveSpeed = 3f;
    }

    // Update is called once per frame
    void Update()
    {
		if (CurrentHealth <= 0)
			return;
		//dirX = CrossPlatformInputManager.GetAxisRaw("Horizontal") * moveSpeed;
		dirX = RightButtonHandler.isRightButtonDown && LeftButtonHandler.isLeftButtonDown? 0f:
			(RightButtonHandler.isRightButtonDown? 1f : (LeftButtonHandler.isLeftButtonDown? -1f : 0));
		transform.position += new Vector3(dirX, 0, 0) * Time.deltaTime * moveSpeed;


		/*if (CrossPlatformInputManager.GetButtonDown("Jump"))
			rb.AddForce(Vector2.up * 300f);*/

		//anim.SetBool("isRunning", false);
		if (Mathf.Abs(dirX) > 0) {
			anim.ResetTrigger("attack");
			anim.SetBool("isRunning", true);
			anim.SetBool("isAttacking", false);
			FindObjectOfType<AudioManager>().Play("Walk");
			FindObjectOfType<AudioManager>().Stop("SwordSlash");
		} else {
			anim.SetBool("isRunning", false);
			FindObjectOfType<AudioManager>().Stop("Walk");
		}
			

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

	public float GetCharacterDamage() {
		return CurrentDamage;
	}
	public void TakeDamage(float damage) {
		CurrentHealth -= damage;
		UpdateHealthBar();
		if (CurrentHealth <= 0) {
			Die();
			Debug.Log("Character has been slained!");
			return;
		}
		Debug.Log("Character was damaged!");
	}
	public void Die() {
		dirX = 0f;
		anim.SetBool("isRunning", false);
		anim.SetTrigger("death");
	}
	public void Destroy() {
		Destroy(gameObject);
	}
	private void UpdateHealthBar() {
		var currentHealth = CurrentHealth / (BaseHealth * HealthMultiplier);
		HealthBar.SetHealth(currentHealth);
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Point") {
			CanBeAttacked = !CanBeAttacked;
		}
	}
	void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "Point") {
			CanBeAttacked = !CanBeAttacked;
		}
	}
	public bool CanCharacterBeAttacked() {
		return CanBeAttacked;
	}
	private void RecalculateCurrentDamage() {
		CurrentDamage = (BaseDamage + BaseDamage * BaseDamageMultiplier) * DamageMultiplier;
	}

	public void IncreaseAttackDamageMultiplier(float multiplier) {
		DamageMultiplier = DamageMultiplier + multiplier/100;
		RecalculateCurrentDamage();
	}
	public void IncreaseHPMultiplier(float multiplier) {
		float takenDamage = BaseHealth * HealthMultiplier - CurrentHealth;
		HealthMultiplier = HealthMultiplier + multiplier / 100;
		CurrentHealth = BaseHealth * HealthMultiplier - takenDamage;
		UpdateHealthBar();
	}
	public float GetCurrentHealth() {
		return CurrentHealth;
	}
	#region Leveling
	public void IncreaseTheStat(Stat statType) {
		switch (statType) {
			case Stat.AttackDamage:
				AttackDamageStat++;
				break;
			case Stat.AttackSpeed:
				AttackSpeedStat++;
				transform.GetComponent<CharacterAttack>().IncreaseAttackSpeed(10);
				break;
			case Stat.Health:
				HealthStat++;
				break;
		}
	}
	public int GetStat(Stat statType) {
		switch (statType) {
			case Stat.AttackDamage:
				return AttackDamageStat;
			case Stat.AttackSpeed:
				return AttackSpeedStat;
			case Stat.Health:
				return HealthStat;
			default: return -1;
		}
		
	}
	public void IncreaseLevel() {
		ExpToLevelUp = ExpToLevelUp * 2f;
		Points ++;
		CurrentLevel++;
		LevelingMenu.UpdateLevelText(CurrentLevel);
		LevelingMenu.UpdatePointsText(Points);
	}
	public void IncreaseLevel(int number) {
		CurrentLevel+= number;
	}
	public void DecreaseLevel() {
		CurrentLevel--;
	}
	public void IncreaseExperience(float exp) {
		TotalExperience += exp;
		if (TotalExperience >= ExpToLevelUp)
			IncreaseLevel();
	}
	public int GetPoints() {
		return Points;
	}
	public void DecreasePoints() {
		Points--;
	}
	#endregion
}

public enum Stat {
	AttackSpeed,
	AttackDamage,
	Health
} 
