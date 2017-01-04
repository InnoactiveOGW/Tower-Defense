using UnityEngine;
using System.Collections;

public class Ghost : Enemy {

	int startingHealth = 10;
	int ghostSpeed = 1; //7
	int ghostDamage = 3;
	int ghostValue = 1;

	bool spawning = true;
	bool isAlive  = true;
	bool hit = true;

	void Start () {
		currentHealth = startingHealth; /*Health setzen*/
		speed = ghostSpeed;
		attackDamage = ghostDamage;
		enemyValue = ghostValue;

		target = GameObject.Find ("Path").transform.GetChild (1);
		agent = GetComponent<NavMeshAgent> ();
		agent.speed = speed;
		gateHealth = target.GetComponent <GateHealth>(); /*Zugriff aufs Script Gate Health*/
	}
	void Update(){
		agent.SetDestination (target.position);
	}
}
