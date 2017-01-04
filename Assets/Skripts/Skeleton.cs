using UnityEngine;
using System.Collections;

public class Skeleton : Enemy {

	int startingHealth = 50;
	int skeletonSpeed = 10;
	int skeletonDamage = 5;
	int skeletonValue = 1;

	bool spawning = true;
	bool isAlive  = true;
	bool hit = true;

	void Start () {
		currentHealth = startingHealth; /*Health setzen*/
		speed = skeletonSpeed;
		attackDamage = skeletonDamage;
		enemyValue = skeletonValue;

		target = GameObject.Find ("Path").transform.GetChild (1);
		agent = GetComponent<NavMeshAgent> ();
		agent.speed = speed;
		gateHealth = target.GetComponent <GateHealth>(); /*Zugriff aufs Script Gate Health*/
	}
	void Update(){
		agent.SetDestination (target.position);
	}
}
