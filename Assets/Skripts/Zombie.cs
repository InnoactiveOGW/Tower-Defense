using UnityEngine;
using System.Collections;

public class Zombie : Enemy {

	int startingHealth = 100;
	int zombieSpeed = 1;
	int zombieDamage = 1;
	int zombieValue = 1;
	bool spawning = true;
	bool isAlive  = true;


	void Start () {
		
		currentHealth = startingHealth; /*Health setzen*/
		speed = zombieSpeed;
		attackDamage = zombieDamage;
		enemyValue = zombieValue;
		target = GameObject.Find ("Path").transform.GetChild (1);
		agent = GetComponent<NavMeshAgent> ();
		agent.speed = speed;
		gateHealth = target.GetComponent <GateHealth>(); /*Zugriff aufs Script Gate Health*/
		spawning = false;
	}
	void Update(){
		
	}

	public void kill(){
		isAlive = false;
	}




}
