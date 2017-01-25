using UnityEngine;
using System.Collections;

public class UpgradeBox : MonoBehaviour {

	GameObject camera;

	// Use this for initialization
	void Start () {
		camera = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.LookAt(camera.transform);
	}
}
