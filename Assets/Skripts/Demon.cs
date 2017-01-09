using UnityEngine;
using System.Collections;

public class Demon : Enemy {

	int startingHealth = 200;
	int demonSpeed = 1;//5
	int demonDamage = 4;
	int demonValue = 2;

	void Start () {
		currentHealth = startingHealth; /*Health setzen*/
		speed = demonSpeed;
		attackDamage = demonDamage;
		enemyValue = demonValue;

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
