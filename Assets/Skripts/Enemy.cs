using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int startingHealth = 100; /*Health der Feinde*/
	public int currentHealth;

	public int lives;
	public int speed;
	public int attackDamage = 5; /*Schaden der zugefügt wird bei Kollision mit Tor*/
	GateHealth gateHealth; /*Für Referenz auf public Methode im Script Gate Health*/

	Transform target;
	NavMeshAgent agent;
	Animator anim;

	// Use this for initialization
	void Start () {
		lives = 3;
		target = GameObject.Find ("Path").transform.GetChild (1);
		agent = GetComponent<NavMeshAgent> ();
		gateHealth = target.GetComponent <GateHealth>(); /*Zugriff aufs Script Gate Health*/
		currentHealth = startingHealth; /*Health setzen*/
		anim = this.transform.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		agent.SetDestination (target.position);
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.name == "End") {
			if (gateHealth.currentHealth > 0) {
				gateHealth.takeDamage (attackDamage); /*Falls Kollision und Tor noch Health übrig, Tor Schaden zufügen*/
				//Death (); Sollte später hier gemacht werden
			}
			die ();
		}
		anim.Play ("walk");
	}

	public void damage(int damage){
		lives -= damage;
		if (lives <= 0) {
			die ();
		}
	}

	private void die(){
		//todo get coins
		Destroy (gameObject);
	}
}
