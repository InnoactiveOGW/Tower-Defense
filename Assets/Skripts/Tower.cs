using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

	float fireCooldown;
	float lastShoot;
	float range;

	// Use this for initialization
	void Start () {
		fireCooldown = 2;
		range = 10;
	}
	
	// Update is called once per frame
	void Update () {
		if ((Time.time - lastShoot) >= fireCooldown) {
			fire ();
			lastShoot = Time.time;
		}
		
				
	}

	void fire(){
		Transform target = getNextEnemy ();
		if (target == null) {
			return;
		}
		GameObject bullet = Instantiate(Resources.Load("Bullet"), this.transform.position, this.transform.rotation) as GameObject; 
		if (bullet.GetComponent<Bullet> () != null) {
			bullet.GetComponent<Bullet> ().setTarget (target.transform);
		}
	}

	Transform getNextEnemy(){
		Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

		Enemy nearestEnemy = null;
		float dist = Mathf.Infinity;

		foreach(Enemy e in enemies) {
			float d = Vector3.Distance(this.transform.position, e.transform.position);
			if(nearestEnemy == null || d < dist) {
				nearestEnemy = e;
				dist = d;
			}
		}

		Vector3 dir = nearestEnemy.transform.position - this.transform.position;

		if (dir.magnitude > range) {
			return null;
		}

		return nearestEnemy.transform;
	}
}
