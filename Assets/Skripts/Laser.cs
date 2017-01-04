using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	Vector3 startPoint;
	Vector3 tmpPoint;
	Vector3 finalTarget;

	bool endReached = false;
	bool startReached = false;

	float speed = 15;
	float distance = 0;

	LineRenderer line;

	Enemy enemy;
	float damage;
	float damageRadius;

	// Use this for initialization
	void Start () {
		startPoint = transform.position;
		tmpPoint = transform.position;
		line = GetComponent <LineRenderer> ();
		line.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (distance == 0) {
			return;
		}
		//Berechne neue Start und Endpunkte
		if (!endReached) {
			Vector3 direction = finalTarget - startPoint;
			tmpPoint += direction.normalized * speed * Time.deltaTime;
			if (Vector3.Distance(startPoint, finalTarget) <= Vector3.Distance(startPoint, tmpPoint)) {
				endReached = true;
				tmpPoint = startPoint;
			}
			//Zeichne Linie
			line.SetPosition(0, startPoint);
			line.SetPosition (1, tmpPoint);
		} else if (!startReached) {
			//der if block funktioniert nicht wie ich mir das vorstelle
			Vector3 direction = finalTarget - startPoint;
			tmpPoint += direction.normalized * speed * Time.deltaTime;
			if (0f <= Vector3.Distance(tmpPoint, finalTarget)) {
				startReached = true;
			}
			//Zeichne Linie
			line.SetPosition(0, tmpPoint);
			line.SetPosition (1, finalTarget);
		} else {
			if (damageRadius == 0) {
				enemy.damage ((int)damage);
			} else {
				//TODO area effect
			}
			Destroy (gameObject);
		}

	}

	public void setTarget(Transform target){
		this.finalTarget = target.position;
		distance = Vector3.Distance (startPoint, finalTarget);
	}

	public void setDamageValues(Enemy enemy, float damage, float damageRadius){
		this.enemy = enemy;
		this.damage = damage;
		this.damageRadius = damageRadius;
	}
}
