using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Connection : MonoBehaviour {


	private GridTile a;
	public GridTile A{
		get { return a; }
	}


	private GridTile b;
	public GridTile B{
		get { return b; }
	}

	private List<Point> points;



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


	public GridTile GetOther(GridTile first){

		if(a == first) return b;
		if(b == first) return a;

		throw new Exception("Argument first (GridTile) is not a member of this connection.");



	}














}
