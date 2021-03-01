using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
	public void PlayGame() {
		Time.timeScale = 1f;
		SceneManager.LoadScene("Game");
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void QuitGame() {
		Application.Quit();
	}
	public void ReturnToMainMenu() {
		SceneManager.LoadScene("MainMenu");
		Time.timeScale = 1f;
	}
	public void Continue() {
		SceneManager.LoadScene("Game");
	}
	public void LoadSettingsMenu() {
		SceneManager.LoadScene("SettingsMenu");
	}
	public void StartAgain() {
		Time.timeScale = 1f;
		SceneManager.LoadScene("Game");
	}
	public void LoadHighscores() {
		Time.timeScale = 1f;
		SceneManager.LoadScene("Highscores");
	}
}
