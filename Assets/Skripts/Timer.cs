using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public TextMesh counterText;
	public float seconds, minutes;

	// Use this for initialization
	void Start () {
		counterText = GetComponent<TextMesh> () as TextMesh;
	}

	// Update is called once per frame
	void Update () {
		minutes = (int)(Time.time / 60f);
		seconds = (int)(Time.time % 60f);
		counterText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
	}
}
