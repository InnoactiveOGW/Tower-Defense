using UnityEngine;
using System.Collections;

public class Demon : Enemy {

	int startingHealth = 200;
	int demonSpeed = 5;
	int demonDamage = 2;
	int demonReward = 1;

	void Start () {
		currentHealth = startingHealth; /*Health setzen*/
		speed = demonSpeed;
		attackDamage = demonDamage;
		reward = demonReward;

		target = GameObject.Find ("Path").transform.GetChild (1);
		agent = GetComponent<NavMeshAgent> ();
		agent.speed = speed;
		gateHealth = target.GetComponent <GateHealth>(); /*Zugriff aufs Script Gate Health*/
	}
}
