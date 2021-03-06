using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
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
	private bool IsAttacking;
	private bool CanAttack;
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
	void Start() {
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

		IsAttacking = false;
		CanAttack = true;

		UpdateHealthBar();
		moveSpeed = 3f;
	}

	// Update is called once per frame
	void Update() {

		if (CurrentHealth <= 0)
			return;
		dirX = RightButtonHandler.isRightButtonDown && LeftButtonHandler.isLeftButtonDown ? 0f :
			(RightButtonHandler.isRightButtonDown ? 1f : (LeftButtonHandler.isLeftButtonDown ? -1f : 0));
		transform.position += new Vector3(dirX, 0, 0) * Time.deltaTime * moveSpeed;
		if (Mathf.Abs(dirX) > 0) {
			if (IsAttacking)
				IsAttacking = false;
			ChangeAnimationState(EnumMethods.GetDescription(MainCharacterAnimations.MainCharacterRun));
			FindObjectOfType<AudioManager>().Play("Walk");
			FindObjectOfType<AudioManager>().Stop("SwordSlash");
		} else if (!IsAttacking) {
			ChangeAnimationState(EnumMethods.GetDescription(MainCharacterAnimations.MainCharacterIdle));
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
			return;
		}
	}
	public void Die() {
		dirX = 0f;
		IsAttacking = false;
		FindObjectOfType<AudioManager>().StopAllSounds();
		CanAttack = false;
		ChangeAnimationState(EnumMethods.GetDescription(MainCharacterAnimations.MainCharacterDeath));
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
		DamageMultiplier = DamageMultiplier + multiplier / 100;
		RecalculateCurrentDamage();
	}
	public void IncreaseHPMultiplier(float multiplier) {
		float takenDamage = BaseHealth * HealthMultiplier - CurrentHealth;
		HealthMultiplier = HealthMultiplier + multiplier / 100;
		CurrentHealth = BaseHealth * HealthMultiplier - takenDamage;
		UpdateHealthBar();
	}
	public void Heal(float healingAmount) {
		CurrentHealth = CurrentHealth + healingAmount;
		if (CurrentHealth > BaseHealth * HealthMultiplier)
			CurrentHealth = BaseHealth * HealthMultiplier;
		UpdateHealthBar();
	}
	public void Heal(int healingAmount) {
		CurrentHealth = CurrentHealth + BaseHealth * HealthMultiplier * healingAmount / 100;
		if (CurrentHealth > BaseHealth * HealthMultiplier)
			CurrentHealth = BaseHealth * HealthMultiplier;
		UpdateHealthBar();
	}
	public float GetCurrentHealth() {
		return CurrentHealth;
	}
	public bool IsCharacterStationary() {
		return dirX == 0;
	}
	public void SetIsAttacking(bool value) {
		IsAttacking = value;
	}
	public bool GetIsAttacking() {
		return IsAttacking;
	}
	public void SetCanAttack(bool value) {
		CanAttack = value;
	}
	public bool GetCanAttack() {
		return CanAttack;
	}
	#region Leveling
	public void IncreaseTheStat(Stat statType) {
		switch (statType) {
			case Stat.AttackDamage:
				AttackDamageStat++;
				IncreaseAttackDamageMultiplier(10);
				break;
			case Stat.AttackSpeed:
				AttackSpeedStat++;
				transform.GetComponent<CharacterAttack>().IncreaseAttackSpeed(10);
				break;
			case Stat.Life:
				HealthStat++;
				IncreaseHPMultiplier(8);
				break;
		}
	}
	public int GetStat(Stat statType) {
		switch (statType) {
			case Stat.AttackDamage:
				return AttackDamageStat;
			case Stat.AttackSpeed:
				return AttackSpeedStat;
			case Stat.Life:
				return HealthStat;
			default:
				return -1;
		}

	}
	public void IncreaseLevel() {
		ExpToLevelUp = ExpToLevelUp * 2f;
		Points++;
		CurrentLevel++;
		LevelingMenu.UpdateLevelText(CurrentLevel);
		LevelingMenu.UpdatePoints(Points);
		GameControl.Notify(EnumMethods.GetDescription(Notifications.LevelUp));
	}
	public void IncreaseLevel(int number) {
		CurrentLevel += number;
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

	#region Animation 
	private string currentState;
	public void ChangeAnimationState(string newState) {
		if (currentState == newState)
			return;
		anim.Play(newState);
		currentState = newState;
	}
	public float GetTheLenghtOfCurrentAnimation() {
		return anim.GetCurrentAnimatorStateInfo(0).length;
	}
	#endregion
}

