using UnityEngine;
using System.Collections;

public class PlyerMovement : MonoBehaviour {

	public int test;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = transform.position;
		position.x += Input.GetAxis ("Horizontal") * Time.deltaTime;
		position.y += Input.GetAxis ("Vertical") * Time.deltaTime;
		transform.position = position;
	}
}
