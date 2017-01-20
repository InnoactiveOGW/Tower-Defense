using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Coins : MonoBehaviour {

	public TextMesh coinText;
	public int coinCount = 20;

	// Use this for initialization
	void Start () {
		coinText = GetComponent<TextMesh> () as TextMesh;
	}

	// Update is called once per frame
	void Update () {
		coinText.text = coinCount.ToString();
	}

	public void gainCoin (int enemyValue) {
		coinCount += enemyValue;
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
