using UnityEngine;
using System.Collections;

public class Skeleton : Enemy {

	int startingHealth = 50;
	int skeletonSpeed = 1;	//10
	int skeletonDamage = 5;
	int skeletonValue = 1;

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
