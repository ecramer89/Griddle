using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Connection : MonoBehaviour {

	public float changePerSecond = .1f;

	private GridTile a;
	public GridTile A{
		get { return a; }
	}


	private GridTile b;
	public GridTile B{
		get { return b; }
	}

	private List<Point> pointsToA = new List<Point>();
	private List<Point> pointsToB  = new List<Point>();
	private Vector3 toA;
	private Vector3 toB;
	private float numPointsA;
	private float numPointsB;
	private Vector3 nxtPositionA;
	private Vector3 nxtPositionB;





	public void SetGridTile(GridTile tile){
		if(a ==  null){
			a = tile;
			return;
		}


		if(b == null){
			b = tile;
			return;

		}


		throw new Exception("Tile attempting to assign self to a connection that already has two tiles. please check.");

	}


	public void InitiateBuildRoutine(){
		if(a == null || b == null) return;

	
		if(pointsToA.Count > 0|| pointsToB.Count > 0) return;

		Point initialPoint  = (Instantiate(Resources.Load("Prefabs/Point", typeof(GameObject)))
			as GameObject).GetComponent<Point>();

		float diam = initialPoint.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

		initialPoint.transform.position = transform.position;


		toA = (a.transform.position - transform.position);
		toB = (b.transform.position - transform.position);
		float distanceA = toA.magnitude;
		float distanceB = toB.magnitude;


		numPointsA = distanceA/diam;
		numPointsB = (distanceB/diam) - 1;

		toB = toB.normalized * diam;
		toA = toA.normalized * diam;

		nxtPositionA = this.transform.position;
		nxtPositionB = this.transform.position;

		pointsToA.Add(initialPoint);

		StartCoroutine(Build());
	

	}


	private IEnumerator Build(){
		while(pointsToA.Count + pointsToB.Count < numPointsA + numPointsB ){

			if(pointsToA.Count < numPointsA){
				Point point  = (Instantiate(Resources.Load("Prefabs/Point", typeof(GameObject))) 
					as GameObject).GetComponent<Point>();

				point.transform.position = nxtPositionA + toA;
				pointsToA.Add(point);
			}


			if(pointsToB.Count < numPointsB){
				Point point  = (Instantiate(Resources.Load("Prefabs/Point", typeof(GameObject))) 
					as GameObject).GetComponent<Point>();

				point.transform.position = nxtPositionB + toB;
				pointsToB.Add(point);

			}



			yield return new WaitForSeconds(changePerSecond);
		}

	}


	public void InitiateDissolveRoutine(){
		StartCoroutine(Dissolve());

	}


	public IEnumerator Dissolve(){
		while(pointsToA.Count + pointsToB.Count > 0 ){

			if(pointsToA.Count > 0){
				Point point  = pointsToA[0];
				pointsToA.RemoveAt(0);
				GameObject.Destroy(point.gameObject);

			}


			if(pointsToB.Count > 0){
				Point point  = pointsToB[0];
				pointsToB.RemoveAt(0);
				GameObject.Destroy(point.gameObject);
			}



			yield return new WaitForSeconds(changePerSecond);
		}



	}


	public GridTile GetOther(GridTile first){

		if(a == first) return b;
		if(b == first) return a;

		throw new Exception("Argument first (GridTile) is not a member of this connection.");



	}














}
