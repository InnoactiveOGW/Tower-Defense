﻿using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

	private int level;
	private static float spawningfactor = 1;

	private enum wave {WAVE_1 = 1, WAVE_2 = 2, WAVE_3 = 3, WAVE_4 = 4};
	private wave currentWave;

	private static int wave_1 = 5;
	private static int wave_2 = 10;
	private static int wave_3 = 15;
	private static int wave_4 = 25;

	public int timeBetweenSpawn = 1; //in sec
	public int timeBetweenWaves = 2; //in sec

	private float lastSpawn;
	private float lastWave;

	private int enemiesSpawnedPerWave;

	// Use this for initialization
	void Start () {
		level = 1;
		enemiesSpawnedPerWave = 0;
		currentWave = wave.WAVE_1;
	}

	private int getAmountOfEnemiesForWave(wave wave){
		int amount = 0;
		switch (wave) {
		case wave.WAVE_1:
			amount = wave_1;
			break;
		case wave.WAVE_2:
			amount = wave_2;
			break;
		case wave.WAVE_3:
			amount = wave_3;
			break;
		case wave.WAVE_4:
			amount = wave_4;
			break;
		}
		amount = (int)(amount * level * spawningfactor);
		return amount;
	}
	
	// Update is called once per frame
	void Update () {
		if (enemiesSpawnedPerWave < getAmountOfEnemiesForWave (currentWave)) {
			if ((Time.time - lastSpawn) >= timeBetweenSpawn) {
				spawnNewEnemy ();
				enemiesSpawnedPerWave += 1;
				lastSpawn = Time.time;
				if (enemiesSpawnedPerWave == getAmountOfEnemiesForWave (currentWave)) {
					lastWave = Time.time;
				}
			}
		}
		else if(enemiesSpawnedPerWave == getAmountOfEnemiesForWave(currentWave)){
			if ((Time.time - lastWave) >= timeBetweenWaves) {
				if ((currentWave + 1) != wave.WAVE_4) {
					currentWave += 1;
				} else {
					level += 1;
				}
				lastSpawn = Time.time;
				enemiesSpawnedPerWave = 0;
			}
		}
		
	}

	private void spawnNewEnemy(){
		Quaternion spawnPoint = GameObject.Find ("Path").transform.GetChild (0).rotation;
		GameObject go = Instantiate(Resources.Load("Enemy")) as GameObject; 
		go.transform.rotation = spawnPoint;
	}
}
