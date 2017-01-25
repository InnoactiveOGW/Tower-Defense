using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GateHealth : MonoBehaviour {

	//Variablen für Health des Tores und rotes Blinken
	public int startingHealth = 100;
	public int currentHealth;
	public Slider healthSlider;
	public Image damageImage;
	public float flashSpeed = 5f;
	public Color flashColor = new Color(1f, 0f, 0f, 0.1f);

	bool isDead;
	bool damaged;


	// Use this for initialization
	void Start () {
		//Health auf Anfangswert setzen
		currentHealth = startingHealth;
	
	}
	
	// Update is called once per frame
	void Update () {
		//Checkt ob Tor Damage bekommt und lässt Healthanzeige rot blinken falls ja
		if (damaged) {
			damageImage.color = flashColor;
		} else {
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		damaged = false;
	
	}

	//public, kann von anderen aufgerufen werden, falls Tor Schaden zugefügt wird
	public void takeDamage (int amount){
		damaged = true; /*damit es blinkt in der update()*/
		currentHealth -= amount; /*Gesundheit Minus zugefügter Schaden setzen*/
		healthSlider.value = currentHealth; /*Slider im Spiel auf momentane Health setzen*/
	}
}
