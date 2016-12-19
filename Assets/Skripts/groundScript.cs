using UnityEngine;
using System.Collections;

public class groundScript : MonoBehaviour {

	public float scaleFactor = 5.0f;
	Material mat;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.mainTextureScale = new Vector2 (transform.localScale.x / scaleFactor , transform.localScale.z / scaleFactor);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
