using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployAsteroids : MonoBehaviour
{
	public GameObject asteroidPrefab;
	public static float respawnTime = 5f;
	public static float DefaultRespawnTime = 5f;
	private Vector2 screenBounds;
	// Start is called before the first frame update
	void Start() {
		screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
		StartCoroutine(asteroidWave());
	}
	private void spawnEnemy() {
		GameObject gameObj = Instantiate(asteroidPrefab) as GameObject;
		asteroidPrefab.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y);
	}
	IEnumerator asteroidWave() {
		while (true) {
			yield return new WaitForSeconds(respawnTime);
			spawnEnemy();
		}
	}
    // Update is called once per frame
    void Update()
    {
        
    }
}
