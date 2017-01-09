using UnityEngine;
using System.Collections;

public class Ghost : Enemy {

	int startingHealth = 100;
	int ghostSpeed = 1; //7
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
