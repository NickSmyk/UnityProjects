using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameControlScript : MonoBehaviour {
	public GameObject heart1, heart2, heart3, gameOver, character;
	public static int health;
	public static bool isInvulnerable;
	public static int invulnerabilityDuration = 2;
	public static DateTime lastTimeBeingHit;
	public int SecondsBetweenLevelUp = 2;
	int level = 1;
	int maxLevel = 45;
	static Renderer characterRenderer;
	static Color characterColor;

	bool wasSaved;
	bool isWaiting = false;

	public int MaxHighscoresNumber = 10;

	string HighscoreTableName = "HighscoreTable";
	public static GameControlScript instance;

	// Start is called before the first frame update
	void Start() {
		wasSaved = false;
		instance = this;
		characterRenderer = character.GetComponent<Renderer>();
		characterColor = characterRenderer.material.color;
		level = 1;
		DeployAsteroids.respawnTime = DeployAsteroids.DefaultRespawnTime;
		ScoreScript.ScoreMultiplier = ScoreScript.DeafaultScoreMultiplier;
		health = 3;
		if (Time.timeScale == 0f) {
			Time.timeScale = 1f;
		}
		heart1.gameObject.SetActive(true);
		heart2.gameObject.SetActive(true);
		heart3.gameObject.SetActive(true);
		gameOver.gameObject.SetActive(false);
		StartCoroutine(StartTheLevelScaling(level, maxLevel));
	}
	public static void PlayInvulnerabilityAnimation() {
		characterColor.a = 0.5f;
		characterRenderer.material.color = characterColor;
		instance.StartCoroutine("PlayInvulnerabilityAnimationBase");
	}

	IEnumerator PlayInvulnerabilityAnimationBase() {
		yield return new WaitForSeconds(invulnerabilityDuration);
		characterColor.a = 1f;
		characterRenderer.material.color = characterColor;
	}


	public static void OnGettingDamage() {
		if ((DateTime.Now - lastTimeBeingHit).TotalSeconds >= 2)
			isInvulnerable = false;
		else
			isInvulnerable = true;
	}
	// Update is called once per frame
	void Update() {
		if (health > 3)
			health = 3;
		if (Time.timeScale == 0f) {
			return;
		}
		switch (health) {
			case 3:
				heart1.gameObject.SetActive(true);
				heart2.gameObject.SetActive(true);
				heart3.gameObject.SetActive(true);
				break;
			case 2:
				heart1.gameObject.SetActive(true);
				heart2.gameObject.SetActive(true);
				heart3.gameObject.SetActive(false);
				break;
			case 1:
				heart1.gameObject.SetActive(true);
				heart2.gameObject.SetActive(false);
				heart3.gameObject.SetActive(false);
				break;
			case 0:
				heart1.gameObject.SetActive(false);
				heart2.gameObject.SetActive(false);
				heart3.gameObject.SetActive(false);
				FindObjectOfType<AudioManager>().StopAllSounds();
				gameOver.gameObject.SetActive(true);
				Time.timeScale = 0f;
				if (!wasSaved)
					SaveScore(ScoreScript.CurrentScore, HighscoreTableName);
				break;

		}
	}
	public void IncreaseTheDifficalty() {
		DeployAsteroids.respawnTime -= 0.1f;
	}
	public void IncreaseTheScoreMultiplier() {
		ScoreScript.ScoreMultiplier += 0.2f;
	}
	public IEnumerator StartTheLevelScaling(int CurrentLevel, int MaxLevel) {
		while (CurrentLevel < MaxLevel && !isWaiting) {
			isWaiting = true;
			yield return new WaitForSeconds(SecondsBetweenLevelUp);
			if (CurrentLevel >= MaxLevel) {
				break;
			}
			CurrentLevel++;
			IncreaseTheDifficalty();
			IncreaseTheScoreMultiplier();
			isWaiting = false;
		}
	}
	public void SaveScore(int score, string highscoreTableName) {
		var currentHighscores = HighscoreTable.GetHighscores(highscoreTableName).HighscoresList;
		int theLowestScore = currentHighscores.Count == 0 ? 0 : currentHighscores.OrderBy(o => o.Score).Select(o => o.Score).FirstOrDefault();
		if (score > theLowestScore || currentHighscores.Count < MaxHighscoresNumber) {
			if (currentHighscores.Count >= MaxHighscoresNumber) {
				var LowestScore = currentHighscores.OrderBy(o => o.Score).FirstOrDefault();
				currentHighscores.Remove(LowestScore);
			}
			var NewHighscore = new ScoreEntry { Score = score };
			currentHighscores.Add(NewHighscore);
			SaveHighscores(currentHighscores, highscoreTableName);
		}
		wasSaved = true;
	}
	public void SaveHighscores(List<ScoreEntry> highscores, string highscoreTableName) {
		var ListOfHighscores = new Highscores { HighscoresList = highscores };
		string json = JsonUtility.ToJson(ListOfHighscores);
		PlayerPrefs.SetString(highscoreTableName, json);
		PlayerPrefs.Save();
	}
}

