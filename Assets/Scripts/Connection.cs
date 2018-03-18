using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Connection : MonoBehaviour {



	private GridTile a;
	public GridTile A{
		get { return a; }
	}


	private GridTile b;
	public GridTile B{
		get { return b; }
	}


	private List<GameObject> points = new List<GameObject>();



	private bool collapsing;

	private static List<Connection> allConnections;

	private Color color;

	public Color Color{
		get {
			return color;

		} 

	}

	public Sprite open;
	public Sprite closed;

	private Sprite currentSprite;


	public void TurnOff(){
		currentSprite = closed;
		foreach(GameObject point in points){
			point.GetComponent<SpriteRenderer>().sprite = currentSprite;

		}
	}


	public void TurnOn(){
		currentSprite = open;
		foreach(GameObject point in points){
			point.GetComponent<SpriteRenderer>().sprite = currentSprite;

		}
	}




	public void Awake(){
		if(allConnections == null){

			allConnections = new List<Connection>();

		}


		allConnections.Add(this);

	}



	public static List<Connection> AllConnections(){
		return allConnections;

	}


	public void OnDestroy(){
		allConnections.Remove(this);
	}

	//connection is passable if the twotiles A and B are currently rotated towards one another.
	public bool IsPassable(){
		
		if(A.state == TileState.NULL || B.state == TileState.NULL) return false;

		Direction outboundDirection = A.GetComponent<GridTile>().GetOutboundDirectionOf(this);

		return A.directions.Contains(outboundDirection)
			&& B.directions.Contains(outboundDirection.Opposite());
	}


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


	public void SetColor(Color newColor){
		this.color = newColor;
		foreach(GameObject point in points){
			point.GetComponent<SpriteRenderer>().color = newColor;

		}

	}


	public void Update(){

		if(collapsing && points.Count > 0){
			GameObject point = points[0];
			points.RemoveAt(0);
			GameObject.Destroy(point);

			if(points.Count == 0) {
				collapsing = false;

			}
		}




	}


	public void BuildConnectionFrom(GridTile from){
		if(points.Count > 0) return; //already built, building or collapsing a connection.


		if(a == null || b == null) return;
		if(a != from && b != from) return;
		if(a.state == TileState.NULL || b.state == TileState.NULL) return;


	
		GridTile target = a == from ? b: a;
		Bullet bullet = Bullet.FireBulletFromTo(from.gameObject, target.gameObject);
		bullet.buildTrail = true;
		bullet.HandleNewPoint += HoldTrail;


	
	}


	private void HoldTrail(GameObject point){
		point.GetComponent<SpriteRenderer>().color=color;
		point.GetComponent<SpriteRenderer>().sprite=currentSprite;
		points.Add(point);

	}

	//clear immeditely, w/o animation
	public void ClearConnection(){
		while(points.Count > 0){
			GameObject p = points[0];
			GameObject.Destroy(p);
			points.RemoveAt(0);
		}

	}



	public void CollapseConnection(){
		if(points.Count == 0) return; //nothing to collapse
		if(!collapsing) {
			collapsing = true;
		}

	}



	public GridTile GetOther(GridTile first){

		if(a == first) return b;
		if(b == first) return a;

		throw new Exception("Argument first (GridTile) is not a member of this connection.");



	}














}
