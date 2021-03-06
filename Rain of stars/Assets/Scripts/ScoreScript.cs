using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {
	public int PointPerSecond;
	public Text ScoreText;
	public static int CurrentScore;
	public static float ScoreMultiplier = 1;
	public static float DeafaultScoreMultiplier = 1;

	public float ScoreCount;

	// Start is called before the first frame update
	void Start() {
		ScoreMultiplier = 1f;
		DeafaultScoreMultiplier = 1;
	}

	// Update is called once per frame
	void Update() {

		ScoreCount += PointPerSecond * Time.deltaTime * ScoreMultiplier;
		CurrentScore = (int)Mathf.Round(ScoreCount);
		ScoreText.text = Mathf.Round(ScoreCount).ToString();

	}
}
