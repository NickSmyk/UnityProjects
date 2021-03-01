using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour {
	private float BaseHealth = 2000f;
	private float HealthMultiplier = 1;
	private float CurrentHealth;
	public HealthBarScript HealthBar;


	// Start is called before the first frame update
	void Start() {
		CurrentHealth = BaseHealth * HealthMultiplier;

	}

    // Update is called once per frame
    void Update()
    {

	}
	public void TakeDamage(float damage) {
		CurrentHealth -= damage;
		UpdateHealthBar();
		if (CurrentHealth <= 0) {
			//Die();
			Debug.Log("Core has been destroyed");
			return;
		}
		Debug.Log("Core  was damaged!");
	}
	private void UpdateHealthBar() {
		var currentHealth = CurrentHealth / (BaseHealth * HealthMultiplier);
		HealthBar.SetHealth(currentHealth);
	}
	public float GetCurrentHealth() {
		return CurrentHealth;
	}
}
