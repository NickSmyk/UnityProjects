using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelingMenu : MonoBehaviour
{
	// Start is called before the first frame update

	public Transform MainCharacter;
	public GameObject LvlingMenu;
	private Character Character;


	public TextMeshProUGUI AttackSpeedText;
	public TextMeshProUGUI LevelText;
	public TextMeshProUGUI PointsText;

	void Start()
    {
		//LvlingMenu.SetActive(false);
		Character = MainCharacter.GetComponent<Character>();
	}

    // Update is called once per frame
    void Update()
    {

	}


	public void IncreaseAttackSpeed() {
		if(Character.GetPoints() >= 1) {
			Character.IncreaseTheStat(Stat.AttackSpeed);
			Character.DecreasePoints();
			UpdateStats();
		}
	}

	public void OpenLvlingMenu() {
		LvlingMenu.SetActive(true);
	}
	public void BackToTheGame() {
		LvlingMenu.SetActive(false);
	}
	public void UpdateStats() {
		AttackSpeedText.text = Character.GetStat(Stat.AttackSpeed).ToString();
		UpdatePointsText(Character.GetPoints());		
	}
	public void UpdateLevelText(int number) {
		LevelText.text = number.ToString();
	}
	public void UpdatePointsText(int points) {
		PointsText.text = points.ToString();
	}
}
