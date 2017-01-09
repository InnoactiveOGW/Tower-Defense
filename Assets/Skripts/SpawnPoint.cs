using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnPoint : MonoBehaviour {

	private int level = 1;
	private float amountOfEnemies = 0; 

	private float timeBetweenSpawn = 2f; //in sec
	private float timeBetweenWaves = 15f; //in sec

	private float timeSinceLastWave = 0;
	private float timeSinceLastSpawn = 0;

	private int enemiesSpawnedPerWave = 0;

	private enum enemy
	{
		ZOMBIE = 1, DEMON = 2, GHOST = 3, SKELETON = 4
	}
	private enemy[] enemyArray;
	private enemy[] currentWaveArray;
	private bool waveShuffled = false;
	private bool waveActive = true;

	// Use this for initialization
	void Start () {
		enemyArray = new enemy[40];
		int i = 0;
		for(i = 0; i < 10; i++){
			enemyArray [i] = enemy.ZOMBIE;
		}
		for(; i < 20; i++){
			enemyArray [i] = enemy.DEMON;
		}
		for(; i < 30; i++){
			enemyArray [i] = enemy.GHOST;
		}
		for(; i < 40; i++){
			enemyArray [i] = enemy.SKELETON;
		}
		getAmountOfEnemiesForWave ();
	}

	private float getAmountOfEnemiesForWave(){
		if (level <= 10) {
			amountOfEnemies = (float)level * 5;
		} else {
			amountOfEnemies = Mathf.Round (amountOfEnemies * 1.1f);
		}

		return amountOfEnemies;
	}

	// Update is called once per frame
	void Update () {
		if (waveActive) {
			if (timeSinceLastSpawn >= timeBetweenSpawn) {
				enemiesSpawnedPerWave += 1;
				if (enemiesSpawnedPerWave == getAmountOfEnemiesForWave ()) {
					waveActive = false;
					waveShuffled = false;
				}
				if (!waveShuffled && level < 10) {
					calculateCurrentWaveArray ();
					waveShuffled = true;
				}
				spawnNewEnemy ();
				// Randomize spawningTime
				timeBetweenSpawn = Random.Range (0.8f, 2.5f);
				timeSinceLastSpawn = 0;
			} else {
				timeSinceLastSpawn += Time.deltaTime;
			}

		} else {
			if (timeSinceLastWave >= timeBetweenWaves) {
				level += 1;
				enemiesSpawnedPerWave = 0;
				waveActive = true;
				timeSinceLastWave = 0;
			} else {
				timeSinceLastWave += Time.deltaTime;
			}
		}
	}

	private void spawnNewEnemy(){
		Vector3 spawnPoint = GameObject.Find ("Path").transform.GetChild (0).position;
		GameObject go = calculateSpawnedEnemy(); 
		go.GetComponent<NavMeshAgent> ().enabled = false;
		go.transform.position = spawnPoint;
	}

	private GameObject calculateSpawnedEnemy(){
		GameObject returnObject = null;
		if (level > 10) {
			int rand = (int)Mathf.Round (Random.Range (1f, 4f));
			returnObject = getEnemyObject((enemy)rand);
		} else {
			if (level == 9) {
				returnObject = Instantiate (Resources.Load ("Zombie")) as GameObject;
			} else if (level == 10) {
				returnObject = Instantiate (Resources.Load ("Demon")) as GameObject;
			} else {
				// the first 8 waves
				returnObject = getEnemyObject(currentWaveArray[enemiesSpawnedPerWave -1]);

			}
		}
		return returnObject;
	}

	private GameObject getEnemyObject(enemy enemy){
		GameObject returnObject = null;
		switch (enemy) {
		case enemy.ZOMBIE:
			returnObject = Instantiate (Resources.Load ("Zombie")) as GameObject;
			break;
		case enemy.DEMON:
			returnObject = Instantiate (Resources.Load ("Demon")) as GameObject;
			break;
		case enemy.GHOST:
			returnObject = Instantiate (Resources.Load ("Ghost")) as GameObject;
			break;
		case enemy.SKELETON:
			returnObject = Instantiate (Resources.Load ("Skeleton")) as GameObject;
			break;
		}
		return returnObject;
	}

	private void calculateCurrentWaveArray(){
		currentWaveArray = new enemy[(int)amountOfEnemies];
		ArrayList shuffleList = new ArrayList ();
		for (int i = 0; i < currentWaveArray.Length; i++) {
			//currentWaveArray [i] = enemyArray [i];
			shuffleList.Add (enemyArray [i]);
		}
		for (int i = 0; i < currentWaveArray.Length; i++) {
			int randomIndex = Random.Range(0, shuffleList.Count);
			currentWaveArray [i] = (enemy)shuffleList [randomIndex];
			shuffleList.RemoveAt (randomIndex);
		}
	}


}
