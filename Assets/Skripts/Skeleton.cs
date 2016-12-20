using UnityEngine;
using System.Collections;

public class Skeleton : Enemy {

	int startingHealth = 50;
	int skeletonSpeed = 10;
	int skeletonDamage = 5;
	int skeletonReward = 1;

	void Start () {
		currentHealth = startingHealth; /*Health setzen*/
		speed = skeletonSpeed;
		attackDamage = skeletonDamage;
		reward = skeletonReward;

		target = GameObject.Find ("Path").transform.GetChild (1);
		agent = GetComponent<NavMeshAgent> ();
		agent.speed = speed;
		gateHealth = target.GetComponent <GateHealth>(); /*Zugriff aufs Script Gate Health*/
	}
}
