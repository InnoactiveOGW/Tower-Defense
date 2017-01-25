using UnityEngine;
using System.Collections;

public class HouseHealth : MonoBehaviour {

	int health;
	bool gameOver;
	int damage;
	public AudioSource demolitionSound;
	public AudioSource gameOverSound;


	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer> ().material.color = Color.green;
		health = 100;
		gameOver = false;
		/*AudioSource[] sounds = GetComponents(AudioSource);
		demolitionSound = sounds [1];
		gameOverSound = sounds [0];*/
	}
	
	// Update is called once per frame
	void Update () {
		if (damage > 0) {
			gameObject.transform.localScale -= new Vector3(0.1F, 0 ,0);
			if (health < 50) gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
			if (health < 20) gameObject.GetComponent<Renderer> ().material.color = Color.red;
			if (health <= 0) {
				gameOver = true;
				gameOverSound.Play ();
				damage = 0;
				gameObject.transform.localScale = new Vector3(0.1F, 0 ,0);
			}

			damage--;
		} 
	}

	public void takeDamage (int amount){
		
		health -= amount; /*Gesundheit Minus zugefügter Schaden setzen*/
		damage = amount;
		demolitionSound.Play ();


	}

	public bool getGameOver(){
		return gameOver;
	}
}
