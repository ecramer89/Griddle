using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour {

	public float unitsPerSecond = 1000f;
	public float pointDiam = 1f;

	private GameObject target;
	private Vector3 trajectory = Vector3.zero;
	public Vector3 Trajectory{
		get { return trajectory; }

	}
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

	public void Start(){
		GetComponent<SpriteRenderer>().color = Settings.global.tileOnColor;

	}





	public void SetTarget(GameObject target){
		this.target = target;
		UpdateTrajectory();
	
	}


	private void UpdateTrajectory(){
		
		trajectory = target.transform.position - gameObject.transform.position;
		trajectory = trajectory.normalized;
	}


	
	// Update is called once per frame
	void Update () {
		
		if(target == null) return;

		UpdateTrajectory();

		Vector3 nextPosition = this.transform.position + (Time.deltaTime * unitsPerSecond * trajectory);
	    
		if(buildTrail) {
			
			//for(float i = 0; i <= (nextPosition - transform.position).magnitude; i++){
				GameObject nxt = (Instantiate(Resources.Load("Prefabs/Point", typeof(GameObject))) 
					as GameObject);
				
				//nxt.transform.localScale = nxt.transform.localScale; * .5f;

			    //randomly jitter the bullets a bit
			nxt.transform.position = nextPosition; //+ new Vector3(UnityEngine.Random.Range(-.25f, .25f), UnityEngine.Random.Range(-.25f, .25f), 0);
				HandleNewPoint(nxt);


			//}
		}


		this.transform.position = nextPosition;


	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.Equals(target)) {
			GameObject.Destroy(gameObject);
		}
	}



}
