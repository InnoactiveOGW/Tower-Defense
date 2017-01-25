using UnityEngine;
using System.Collections;

public class Zombie : Enemy {

	int startingHealth = 30;
	int zombieSpeed = 1;
	int zombieDamage = 2;
	int zombieValue = 2;



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
				LaserTowerPlacement ltp = null;
				if (GameObject.Find ("Controller (left)") == null) {
					ltp = GameObject.Find ("Controller (right)").transform.GetComponent<LaserTowerPlacement> ();
				} else{
					ltp = GameObject.Find ("Controller (left)").transform.GetComponent<LaserTowerPlacement> ();
				}
				if (ltp != null) {
					ltp.updateUsedSpaceAt (nextTower.transform, 0);
					GameObject explosion = Instantiate (Resources.Load ("TowerExplosion")) as GameObject;
					explosion.transform.position = nextTower.transform.position;
					explosion.SetActive (true);
					Destroy (nextTower);
				}
			}
		}
	}

}
