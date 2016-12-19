using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	int damage;
	int speed;

	Transform target;

	// Use this for initialization
	void Start () {
		damage = 1;
		speed = 2;

	}

	// Update is called once per frame
	void Update () {
		if (target == null) {
			Destroy (gameObject);
			return;
		}

		Vector3 direction = target.position - this.transform.position;
		transform.Translate( direction.normalized * Time.deltaTime * speed, Space.World );
		Quaternion targetRotation = Quaternion.LookRotation( direction );
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime*5);

	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.name == "Enemy(Clone)") {
			GameObject enemy = col.gameObject;
			doDamage (enemy);
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
}
