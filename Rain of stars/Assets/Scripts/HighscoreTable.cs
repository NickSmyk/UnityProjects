using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour {
	private Transform entryContainer;
	private Transform entryTemplate;
	private float TemplateHeight = 100f;

	string HighscoreTableName = "HighscoreTable";

	private void Awake() {
		entryContainer = transform.Find("HighscoreEntyContainer");
		entryTemplate = entryContainer.Find("HighscoreEntryTemplate");
		TemplateHeight = entryTemplate.transform.GetComponent<RectTransform>().rect.height;
		entryTemplate.gameObject.SetActive(false);

		var highscores = GetHighscores(HighscoreTableName).HighscoresList;
		FillTheTable(entryTemplate, entryContainer, highscores);
	}
	public void FillTheTable(Transform template, Transform container, List<ScoreEntry> listOfHighscores) {
		int rank = 1;
		var highscores = listOfHighscores.OrderByDescending(o => o.Score).ToList();
		foreach (var highscore in highscores) {
			Transform entryTransform = Instantiate(template, container);
			RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
			entryRectTransform.anchoredPosition = new Vector2(0, -TemplateHeight * (rank - 1));
			entryTransform.gameObject.SetActive(true);

			string rankString;
			switch (rank) {
				default:
					rankString = rank + "TH";
					break;
				case 1:
					rankString = "1ST";
					break;
				case 2:
					rankString = "2ND";
					break;
				case 3:
					rankString = "3RD";
					break;
			}
			entryTransform.Find("Position").GetComponent<Text>().text = rankString;

			int score = highscore.Score;
			entryTransform.Find("Score").GetComponent<Text>().text = score.ToString();
			rank++;

		}

	}

	public static Highscores GetHighscores(string highscoreTableName) {
		string jsonString = PlayerPrefs.GetString(highscoreTableName);
		Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString) ?? new Highscores();
		return highscores;
	}
}

public class Highscores {
	public List<ScoreEntry> HighscoresList = new List<ScoreEntry>();
}

[System.Serializable]
public class ScoreEntry {
	public int Score;
}