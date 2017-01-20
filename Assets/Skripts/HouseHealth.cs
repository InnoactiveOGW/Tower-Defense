using UnityEngine;
using System.Collections;

public class HouseHealth : MonoBehaviour {

	int health;
	bool gameOver;
	int damage;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer> ().material.color = Color.green;
		health = 100;
		gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (damage > 0) {
			gameObject.transform.localScale -= new Vector3(0.1F, 0 ,0);
			if (health < 50) gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
			if (health < 20) gameObject.GetComponent<Renderer> ().material.color = Color.red;
			if (health <= 0) {
				gameOver = true;
				damage = 0;
				gameObject.transform.localScale = new Vector3(0.1F, 0 ,0);
			}

			damage--;
		} 
	}

	public void takeDamage (int amount){
		
		health -= amount; /*Gesundheit Minus zugefügter Schaden setzen*/
		damage = amount;

	}

	public bool getGameOver(){
		return gameOver;
	}
}
