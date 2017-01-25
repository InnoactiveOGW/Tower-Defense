using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	int damage = 1;
	int speed = 15;

	Transform target;
	public float radius = 0;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (target == null) {
			Destroy (gameObject);
			return;
		}
		Vector3 direction = target.position - this.transform.localPosition;
		float distance = speed * Time.deltaTime;

		if(direction.magnitude <= distance) {
			DoBulletHit();
		}
		else {
			transform.Translate( direction.normalized * distance, Space.World );
			Quaternion targetRotation = Quaternion.LookRotation( direction );
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime*5);
		}

	}

	void doDamage(GameObject enemy){
		if(enemy.GetComponent<Enemy>() != null){
			enemy.GetComponent<Enemy>().damage (this.damage);
			Destroy (gameObject);
		}
	}

	public void setTarget(Transform target){
		this.target = target;
	}

	void DoBulletHit() {
		if(radius == 0) {
			target.GetComponent<Enemy>().damage(damage);
		}
		else {
			Collider[] cols = Physics.OverlapSphere(transform.position, radius);

			foreach(Collider c in cols) {
				Enemy enemy = c.GetComponent<Enemy>();
				if(enemy != null) {
					enemy.GetComponent<Enemy>().damage(damage);
				}
			}
		}
		Destroy(gameObject);
	}
}
