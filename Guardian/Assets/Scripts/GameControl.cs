using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{

	public Transform LevelingMenuButton;
	private bool IsLevelingMenuButtonBlinking;
	private float StartBlinkCD = 1f;
	private float BlinkCD;
	private Image LevelingButtonImage;

	public GameObject gameOver;
	public Transform IncreaseHP;
	public Transform PowerAttack;
	public Transform AttackSpeed;

	public Transform TheSpawnPoint;
	public Character MainCharacter;
	public Core TheCore;
	public static string HighscoreTableName = "HighscroreTableProtector";

	public TextMeshProUGUI Score;
	public int MaxHighscoresNumber = 10;

	public static GameControl Instance;
	private static int ScoreCount = 0;

	private bool wasSaved;
	private bool isGameOver;

	// Start is called before the first frame update
	void Start()
    {
		Instance = this;
		ScoreCount = 0;
		Score.text = ScoreCount.ToString();
		wasSaved = false;
		isGameOver = false;

		LevelingButtonImage = LevelingMenuButton.GetComponent<Image>();
		BlinkCD = StartBlinkCD;
		//MainCharacter = gameObject.GetComponent<Character>();
	}

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver && (MainCharacter.GetCurrentHealth() <= 0 || TheCore.GetCurrentHealth() <= 0)) {
			GameIsOver();
		}

		if (MainCharacter.GetPoints() == 0) {
			if(LevelingButtonImage.color.a != 1)
			LevelingButtonImage.color =  new Color(LevelingButtonImage.color.r, LevelingButtonImage.color.g, LevelingButtonImage.color.b, 1);
			return;
		}
		if (BlinkCD <= 0) {
			switch (LevelingButtonImage.color.a) {
				case 0.5f:
					LevelingButtonImage.color = new Color(LevelingButtonImage.color.r, LevelingButtonImage.color.g, LevelingButtonImage.color.b, 1);
					break;
				case 1:
					LevelingButtonImage.color = new Color(LevelingButtonImage.color.r, LevelingButtonImage.color.g, LevelingButtonImage.color.b, 0.5f);
					break;
			}
			BlinkCD = StartBlinkCD;
		} else {
			BlinkCD -= Time.deltaTime;
		}
	}

	private void GameIsOver() {
		gameOver.SetActive(true);
		//Time.timeScale = 0f;
		if (!wasSaved)
			SaveScore(ScoreCount, HighscoreTableName);
		wasSaved = true;
		isGameOver = true;
	}
	public bool IsGameOver() {
		return isGameOver;
	}

	public static void DropAnItem(Transform Object) {
		int itemNumber = Random.Range(0, 4);
		switch (itemNumber) {
			case 1:
				SpawnItem(Instance.IncreaseHP, Object);
				break;
			case 2:
				SpawnItem(Instance.PowerAttack, Object);
				break;
			case 3:
				SpawnItem(Instance.AttackSpeed, Object);
				break;
		}


	}
	public static void GrantExperience(float exp) {
		Instance.MainCharacter.IncreaseExperience(exp);
	}
	#region Score
	public static void IncreaseScore(int number) {
		for(int i =0; i<number; i++) {
			IncreaseScore();
		}
		UpdateScore();
	}
	public static void IncreaseScore() {
		ScoreCount++;
	}
	public static void UpdateScore() {
		Instance.Score.text = ScoreCount.ToString();
	}
	public static int GetScore() {
		return ScoreCount;
	}
	#endregion
	public static void SpawnItem(Transform item, Transform Object) {
		Vector3 position = Object.position;
		//Debug.Log(position.x);
		float xBound = GetTheBoundX(Instance.TheSpawnPoint);
		if (position.x > xBound)
			position.x = xBound;
		Instantiate(item, position, Object.rotation);
	}
	public static float GetTheBoundX(Transform theSpawnPoint) {
		float xBound = theSpawnPoint.position.x - theSpawnPoint.GetComponent<CircleCollider2D>().bounds.size.x / 2;
		//Debug.Log(xBound);
		return xBound;
	}
	#region Score saving
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
	}
	public void SaveHighscores(List<ScoreEntry> highscores, string highscoreTableName) {
		var ListOfHighscores = new Highscores { HighscoresList = highscores };
		string json = JsonUtility.ToJson(ListOfHighscores);
		PlayerPrefs.SetString(highscoreTableName, json);
		PlayerPrefs.Save();
	}
	#endregion
}
