using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
	public GameObject pauseMenu;


	public void PauseTheGame() {
		pauseMenu.SetActive(true);
		Time.timeScale = 0f;
	}
	public void ResumeTheGame() {
		pauseMenu.SetActive(false);
		Time.timeScale = 1f;
	}
}
