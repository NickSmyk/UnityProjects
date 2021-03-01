using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
		//pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void MainMenu() {
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");
		Screen.orientation = ScreenOrientation.Portrait;
	}
	public void StartAgain() {
		Time.timeScale = 1f;
		SceneManager.LoadScene("Game");
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}

	public void PauseTheGame() {
		pauseMenu.SetActive(true);
		Time.timeScale = 0f;
	}
	public void ResumeTheGame() {
		pauseMenu.SetActive(false);
		Time.timeScale = 1f;
	}
}
