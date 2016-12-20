﻿using UnityEngine;
using System.Collections;

public class Zombie : Enemy {

	int startingHealth = 100;
	int zombieSpeed = 1;
	int zombieDamage = 1;
	int zombieReward = 1;

	void Start () {
		currentHealth = startingHealth; /*Health setzen*/
		speed = zombieSpeed;
		attackDamage = zombieDamage;
		reward = zombieReward;

		target = GameObject.Find ("Path").transform.GetChild (1);
		agent = GetComponent<NavMeshAgent> ();
		agent.speed = speed;
		gateHealth = target.GetComponent <GateHealth>(); /*Zugriff aufs Script Gate Health*/
	}


}
