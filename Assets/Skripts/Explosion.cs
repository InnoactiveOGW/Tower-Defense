using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	float expolsionDestroyDelay = 3f;
	float time = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (time >= expolsionDestroyDelay) {
			Destroy (gameObject);
		} 
		else {
			time += Time.deltaTime;
		}
	}
}
