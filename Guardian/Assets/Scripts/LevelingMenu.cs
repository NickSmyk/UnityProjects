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
	public Transform AttackSpeedButton;
	public TextMeshProUGUI AttackDamageText;
	public Transform AttackDamageButton;
	public TextMeshProUGUI MaximumLifeText;
	public Transform MaximumLifeButton;

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
	public void IncreaseMaximumLife() {
		if (Character.GetPoints() >= 1) {
			Character.IncreaseTheStat(Stat.Life);
			Character.DecreasePoints();
			UpdateStats();
		}
	}
	public void IncreaseAttackDamage() {
		if (Character.GetPoints() >= 1) {
			Character.IncreaseTheStat(Stat.AttackDamage);
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
		AttackDamageText.text = Character.GetStat(Stat.AttackDamage).ToString();
		MaximumLifeText.text = Character.GetStat(Stat.Life).ToString();
		UpdatePoints(Character.GetPoints());		
	}
	public void UpdateLevelText(int number) {
		LevelText.text = number.ToString();
	}
	public void UpdatePoints(int points) {
		if(points == 0) {
			SetAttackDamageButtonInteractable(false);
			SetAttackSpeedButtonInteractable(false);
			SetMaximumLifeButtonInteractable(false);
		} else {
			SetAttackDamageButtonInteractable(true);
			SetAttackSpeedButtonInteractable(!GameControl.IsAttackSpeedMax);
			SetMaximumLifeButtonInteractable(true);
		}
		UpdatePointsText(points);
	}
	public void UpdatePointsText(int points) {
		PointsText.text = points.ToString();
	}
	public void SetAttackDamageButtonInteractable(bool isInteractable) {
		AttackDamageButton.GetComponent<Button>().interactable = isInteractable;
	}
	public void SetAttackSpeedButtonInteractable(bool isInteractable) {
		AttackSpeedButton.GetComponent<Button>().interactable = isInteractable;
	}
	public void SetMaximumLifeButtonInteractable(bool isInteractable) {
		MaximumLifeButton.GetComponent<Button>().interactable = isInteractable;
	}
}
