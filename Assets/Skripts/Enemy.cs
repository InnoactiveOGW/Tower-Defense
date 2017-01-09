using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int currentHealth;
	protected int speed = 1;
	protected int attackDamage; /*Schaden der zugefügt wird bei Kollision mit Tor*/
	protected int enemyValue;

	protected GateHealth gateHealth; /*Für Referenz auf public Methode im Script Gate Health*/
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
		gateHealth = target.GetComponent <GateHealth>(); /*Zugriff aufs Script Gate Health*/
	}

	// Update is called once per frame
	void Update () {
		agent.enabled = true;
		agent.SetDestination (target.position);
		this.spawning = false;
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.name == "End") {
			if (gateHealth.currentHealth > 0) {
				gateHealth.takeDamage (attackDamage); /*Falls Kollision und Tor noch Health übrig, Tor Schaden zufügen*/
				//Death (); Sollte später hier gemacht werden
			}
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

	private void die(){
		GameObject coinCounter = GameObject.Find("CoinCounter");
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
