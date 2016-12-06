using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int lives;
	public int speed;

	Transform target;
	NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Path").transform.GetChild (1);
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		agent.SetDestination (target.position);
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.name == "End") {
			Destroy (gameObject);
		}


	}
}
