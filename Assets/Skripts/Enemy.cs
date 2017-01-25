using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	protected int currentHealth;
	protected int speed = 1;
	protected int attackDamage; /*Schaden der zugefügt wird bei Kollision mit Tor*/
	protected int enemyValue;

	protected HouseHealth houseHealth; /*Für Referenz auf public Methode im Script HouseHealth*/
	protected Transform target;
	protected NavMeshAgent agent;

	protected bool spawning = true;
	protected bool isAlive  = true;
	protected bool hit = false;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Path").transform.GetChild (1);
		agent = GetComponent<NavMeshAgent> ();
		agent.speed = speed;

		GameObject healthBar = GameObject.Find ("HealthBar");
		houseHealth = healthBar.GetComponent <HouseHealth>(); /*Zugriff aufs Script Gate Health*/
	}

	// Update is called once per frame
	void Update () {
		agent.enabled = true;
		agent.SetDestination (target.position);
		this.spawning = false;
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.name == "End") {
			houseHealth.takeDamage (attackDamage); /*Falls Kollision und Tor noch Health übrig, Tor Schaden zufügen*/
				//Death (); Sollte später hier gemacht werden
			isAlive = false;
			// die() darf hier nicht aufgerufen werden, da sonst Coins hochgezählt werden
			Destroy (gameObject);
		}
	}

	public void damage(int damage){
		currentHealth -= damage;
		if (currentHealth <= 0) {
			isAlive = false;
			die ();
		}
	}

	protected void die(){
		GameObject coinCounter = GameObject.Find("CoinText");
		Coins coins = coinCounter.GetComponent <Coins>(); /*Zugriff aufs Script Coins*/
		coins.gainCoin (enemyValue);
		Destroy (gameObject);
	}

	protected GameObject getNextTower(){
		Ray ray = new Ray (transform.position, target.position-transform.position);
		RaycastHit raycastHit;
		if(Physics.Raycast (ray, out raycastHit, 1/*, LayerMask.GetMask ("Tower")*/))
		{
			if (raycastHit.collider.GetComponent<Tower> () != null) {
				return raycastHit.transform.gameObject;
			}
		}
		return null;
	}
}
