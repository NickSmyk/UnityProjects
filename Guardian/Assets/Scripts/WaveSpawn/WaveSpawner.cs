using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
	private enum SpawnState { Spawning, Waiting, Counting };

	public Wave[] Waves;
	public Transform[] SpawnPoints;
	public int NextWave = 0;
	public float TimeBetweenWaves = 5f;
	private float WaveCountdown;
	private SpawnState State = SpawnState.Counting;
	private float SearchCountdown = 1f;


	public Wave Wave;
	private int CurrentLevel = 1;
	private int NumberOfWaves = 2;
	private int CurrentWave = 1;
	private float EnemyHealthMultiplier;
	private float EnemyDamageMultiplier;
	private float EnemyExperienceMultiplier;

	private void Start() {
		WaveCountdown = TimeBetweenWaves;
		EnemyHealthMultiplier = 1f;
		EnemyDamageMultiplier = 1f;
	}

	private void Update() {
		if (State == SpawnState.Waiting) {
			if (!EnemyIsAlive()) {
				if (CurrentWave - 1 >= NumberOfWaves) {
					CurrentWave = 1;
					WaveComplited();
				}
			} else
				return;
		}
		if (WaveCountdown <= 0 && State != SpawnState.Spawning) {
			StartCoroutine(SpawnWave(Wave));
		} else if (WaveCountdown > 0) {
			WaveCountdown -= Time.deltaTime;
		}
	}

	bool EnemyIsAlive() {
		SearchCountdown -= Time.deltaTime;
		if (SearchCountdown <= 0f) {
			SearchCountdown = 1f;
			if (GameObject.FindGameObjectWithTag("Enemy") == null) {
				return false;
			}
		}
		return true;
	}
	#region Enemy Spawn
	IEnumerator SpawnWave(Wave Wave) {
		State = SpawnState.Spawning;
		if (CurrentWave == 1)
			yield return new WaitForSeconds(5f);
		for (int i = 0; i < Wave.Count; i++) {
			SpawnEnemy(Wave.Enemy, i);
			yield return new WaitForSeconds(1f / Wave.Rate);
		}
		CurrentWave++;

		State = SpawnState.Waiting;
		yield break;
	}


	void SpawnEnemy(Transform enemy, int enemyNumber) {
		if (SpawnPoints.Length == 0) {
			return;
		}
		Transform sp = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)];
		Vector3 postition = new Vector3(sp.position.x, (sp.position.y - UnityEngine.Random.Range(-0.7f, 0.7f)), sp.position.z);
		enemy.GetComponent<Renderer>().sortingOrder = enemyNumber + 3;
		Instantiate(enemy, postition, sp.rotation);
	}
	#endregion

	void WaveComplited() {
		if (CurrentLevel % 5 == 0) {
			EnemyHealthMultiplier += 0.5f;
			EnemyDamageMultiplier += 0.1f;
			EnemyExperienceMultiplier += 1;
			Wave.Enemy.GetComponent<EnemyScript>().SetDamageMultiplier(EnemyDamageMultiplier);
			Wave.Enemy.GetComponent<EnemyScript>().SetHealthMultiplier(EnemyHealthMultiplier);
			Wave.Enemy.GetComponent<EnemyScript>().SetExperienceMultiplier(EnemyExperienceMultiplier);
			GameControl.Notify(EnumMethods.GetDescription(Notifications.EnemiesAreStronger));
			Wave.Count -= 3;
			NumberOfWaves++;

		}
		Wave.Count++;
		CurrentLevel++;
	}

}

[Serializable]
public class Wave {
	string Name;
	public Transform Enemy;
	public int Count;
	public float Rate;

}