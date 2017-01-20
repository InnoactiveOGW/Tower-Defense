using UnityEngine;
using System.Collections;

public class Zombie : Enemy {

	int startingHealth = 5;
	int zombieSpeed = 1;
	int zombieDamage = 10;
	int zombieValue = 1;


	void Start () {
		currentHealth = startingHealth; /*Health setzen*/
		speed = zombieSpeed;
		attackDamage = zombieDamage;
		enemyValue = zombieValue;
		target = GameObject.Find ("Path").transform.GetChild (1);
		agent = GetComponent<NavMeshAgent> ();
		agent.speed = speed;
		GameObject healthBar = GameObject.Find ("HealthBar");
		houseHealth = healthBar.GetComponent <HouseHealth>(); /*Zugriff aufs Script Gate Health*/
		spawning = false;
	}

	void Update(){
		agent.enabled = true;
		agent.SetDestination (target.position);

		NavMeshPath path = new NavMeshPath ();
		agent.CalculatePath (target.transform.position, path);
		//Debug.Log (path.status);
		if(path.status == NavMeshPathStatus.PathPartial){
			GameObject nextTower = getNextTower ();
			if (nextTower != null) {
				GroundTowerPlacement gtp = GameObject.Find ("_Skripts_").transform.GetComponent<GroundTowerPlacement> ();
				gtp.updateUsedSpaceAt (nextTower.transform, 0);
				Destroy (nextTower);
			}
		}
	}

}
