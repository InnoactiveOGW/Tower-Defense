using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	protected int currentHealth;
	protected int speed = 1;
	protected int attackDamage; /*Schaden der zugefügt wird bei Kollision mit Tor*/
	protected int reward;

<<<<<<< HEAD
	public int lives;
	public int speed;
	public int attackDamage = 5; /*Schaden der zugefügt wird bei Kollision mit Tor*/
	GateHealth gateHealth; /*Für Referenz auf public Methode im Script Gate Health*/

	Transform target;
	NavMeshAgent agent;
	Animator anim;
=======
	protected GateHealth gateHealth; /*Für Referenz auf public Methode im Script Gate Health*/
	protected Transform target;
	protected NavMeshAgent agent;
>>>>>>> master

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Path").transform.GetChild (1);
		agent = GetComponent<NavMeshAgent> ();
		agent.speed = speed;
		gateHealth = target.GetComponent <GateHealth>(); /*Zugriff aufs Script Gate Health*/
<<<<<<< HEAD
		currentHealth = startingHealth; /*Health setzen*/
		anim = this.transform.GetComponent<Animator>();
=======
>>>>>>> master
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
		currentHealth -= damage;
		if (currentHealth <= 0) {
			die ();
		}
	}

	private void die(){
		//todo get coins
		Destroy (gameObject);
	}
}
