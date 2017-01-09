using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

	float fireSpeed = 5;
	float range = 3;
	float damage = 5;
	float damageRadius = 2;

	float timeSinceLastShot = 0;

	//Variablen für shoot()
	//ParticleSystem gunParticles;
	LineRenderer gunLine;
	int shootableMask;
	Light gunLight;
	Ray shootRay; // A ray from the gun end forwards
	RaycastHit shootHit; // A raycast hit to get information about what was hit

	float effectsDisplayTime = 0.1f;

	// Use this for initialization
	void Start () {
		// Create a layer mask for the Shootable layer.
		shootableMask = LayerMask.GetMask ("Shootable");

		// Set up the references.
		//gunParticles = GetComponent<ParticleSystem> ();
		//gunLight = GetComponent<Light> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (timeSinceLastShot >= fireSpeed) {
			shoot ();
			timeSinceLastShot = 0;
		} 
		else {
			timeSinceLastShot += Time.deltaTime;
		}

		if(timeSinceLastShot >= fireSpeed * effectsDisplayTime) {
			DisableEffects ();
		}
	}

	private void DisableEffects (){
		//gunLight.enabled = false;
		//gunLine.enabled = false;
	}

	private void shoot(){
		//Laser Schuss
		Transform target = getNextEnemy ();
		if (target == null) {
			return;
		}

		//gunLight.enabled = true;

		//gunParticles.Stop ();
		//gunParticles.Play ();

		Vector3 pos = this.transform.position;
		pos.y = pos.y + 1.25f;
		Vector3 crystalPosition = pos;//position of crystal
		//shootRay.origin = transform.position;
		shootRay.origin = crystalPosition;
		//shootRay.direction = target.transform.position - transform.position;
		shootRay.direction = target.transform.position - crystalPosition;

		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			Enemy enemy = shootHit.collider.GetComponent <Enemy> ();
			if(enemy != null)
			{
				GameObject laser = Instantiate(Resources.Load("Laser"), crystalPosition, this.transform.rotation) as GameObject;
				if (laser.GetComponent<Laser> () != null) {
					laser.GetComponent<Laser> ().setTarget (target.transform);
					laser.GetComponent<Laser> ().setDamageValues (enemy, damage, damageRadius);
				}
			}
		}

		/*
		//Kugel Schuss 
		Transform target = getNextEnemy ();
		if (target == null) {
			return;
		}
		GameObject bullet = Instantiate(Resources.Load("Bullet"), this.transform.position, this.transform.rotation) as GameObject; 
		if (bullet.GetComponent<Bullet> () != null) {
			bullet.GetComponent<Bullet> ().setTarget (target.transform);
		}
		*/
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
		if (nearestEnemy != null) {
			Vector3 dir = nearestEnemy.transform.position - this.transform.position;

			if (dir.magnitude > range) {
				return null;
			}

			return nearestEnemy.transform;
		}
		return null;
	}

	public void upgradeSpeed(){
		fireSpeed = 2;
		//TODO update Tower occurance
	}

	public void upgradeDamage(){
		damage = 10;
		//TODO update Tower occurance
	}

	public void upgradeArea(){
		damageRadius = 2;
		//TODO update Tower occurance
	}
}
