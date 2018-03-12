using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour {

	public float unitsPerSecond = 8f;
	public float pointDiam = 1f;

	private GameObject target;
	private Vector3 trajectory = Vector3.zero;
	[HideInInspector]
	public bool buildTrail;



	public event Action<GameObject> HandleNewPoint = (GameObject p) => {};

	public static Bullet FireBulletFromTo(GameObject from, GameObject to){
		Bullet bullet = (Instantiate(Resources.Load("Prefabs/Bullet", typeof(GameObject))) 
			as GameObject).GetComponent<Bullet>();

		bullet.transform.position = from.transform.position;
		bullet.SetTarget(to);
		return bullet;
	
	}





	public void SetTarget(GameObject target){
		this.target = target;
		trajectory = target.transform.position - gameObject.transform.position;
		trajectory = trajectory.normalized;
	}



	
	// Update is called once per frame
	void Update () {

		Vector3 nextPosition = this.transform.position + (Time.deltaTime * unitsPerSecond * trajectory);
	    
		if(buildTrail) {
			float diam = 0;
			for(float i = 0; i <= (nextPosition - transform.position).magnitude; i++){
				GameObject nxt = (Instantiate(Resources.Load("Prefabs/Glow", typeof(GameObject))) 
					as GameObject);
				

				nxt.GetComponent<Glow>().UpateScale(nxt.transform.localScale * .25f);

				nxt.transform.position = nextPosition + (trajectory * i);
				HandleNewPoint(nxt);


			}
		}


		this.transform.position = nextPosition;


	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.Equals(target)) GameObject.Destroy(gameObject);
	}



}
