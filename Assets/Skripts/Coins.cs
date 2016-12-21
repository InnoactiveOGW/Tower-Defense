using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Coins : MonoBehaviour {

	public Text coinText;
	int coinCount = 0;

	// Use this for initialization
	void Start () {
		coinText = GetComponent<Text> () as Text;
	}

	// Update is called once per frame
	void Update () {

	}

	public void gainCoin (int enemyValue) {
		coinCount += enemyValue;
		coinText.text = coinCount.ToString();
	}
}
