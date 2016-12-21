﻿using UnityEngine;
using System.Collections;

public class Ghost : Enemy {

	int startingHealth = 10;
	int ghostSpeed = 7;
	int ghostDamage = 3;
	int ghostValue = 1;

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
}
