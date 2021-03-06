using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame() {
		SceneManager.LoadScene("Game");
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
	public void GoToTheMainMenu() {
		SceneManager.LoadScene("MainMenu");
		Screen.orientation = ScreenOrientation.Portrait;
	}
	public void Highscores() {
		SceneManager.LoadScene("Highscores");
	}
	public void AboutTheGame() {
		SceneManager.LoadScene("AboutTheGame");
	}
	public void Quit() {
		Application.Quit();
	}

}
