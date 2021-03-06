using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {
	public Slider Slider;

	// Start is called before the first frame update
	public void SetHealth(float health) {
		Slider.value = health;
	}
}
