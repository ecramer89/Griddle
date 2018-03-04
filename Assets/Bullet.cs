using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float unitsPerSecond = 8f;

	public float stoppingDistance;

	private GameObject target;
	private Vector3 trajectory = Vector3.zero;


	// Use this for initialization
	void Start () {
	}



	public void SetTarget(GameObject target){
		this.target = target;
		trajectory = target.transform.position - gameObject.transform.position;
		trajectory = trajectory.normalized;
	}



	
	// Update is called once per frame
	void Update () {
		this.transform.position = this.transform.position + (Time.deltaTime * unitsPerSecond * trajectory);

	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.Equals(target)) GameObject.Destroy(gameObject);
	}



}
