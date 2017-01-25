using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Coins : MonoBehaviour {

	public TextMesh coinText;
	int coinCount = 100;
	AudioSource deathSound;

	// Use this for initialization
	void Start () {
		coinText = GetComponent<TextMesh> () as TextMesh;
		deathSound = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		coinText.text = coinCount.ToString();
	}

	public void gainCoin (int enemyValue) {
		coinCount += enemyValue;
		deathSound.Play ();
	}

	public bool isPossibleToBuy(int cost){
		if (coinCount - cost < 0) {
			return false;
		} else {
			return true;
		}
	}

	public void useCoinsToBuy(int cost){
		if (isPossibleToBuy(cost)) {
			coinCount -= cost;
		} else {
			return;
		}
	}
}
