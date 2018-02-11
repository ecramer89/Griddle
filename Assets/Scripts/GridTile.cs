using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public enum Direction{
	NORTH,
	EAST,
	SOUTH, 
	WEST
}

public enum TileState{
	ACTIVE_A,
	ACTVE_B,
	INACTIVE
}

public class GridTile : MonoBehaviour {
	
	public static List<GridTile> TILES_LIST = new List<GridTile>();

	[HideInInspector]
	public Direction[] directions;
	[HideInInspector]
	private SpriteRenderer sprite;
	[HideInInspector]
	public TileState state;
	[HideInInspector]
	public int x;
	[HideInInspector]
	public int y;
	[HideInInspector]
	public Column column;


	void Awake(){
		
		TILES_LIST.Add(this);
	
		sprite = GetComponent<SpriteRenderer>();

	
	}

	//use on mouse over instead of onMouseDown since former only invoked if user presses left mouse button
	public void OnMouseOver(){

		if(Input.GetMouseButtonDown(0)){
			Rotate();
		}
		if(Input.GetMouseButtonDown(1)){
			Game.instance.MakeMove(this);
		}
	}


	public void Rotate(){
		
		transform.Rotate(Vector3.forward * -90);

		for(int i=0;i<directions.Length;i++){
			directions[i]= (Direction)(((int)(directions[i]) + 1) % Enum.GetValues(typeof(Direction)).Length);
		}

	



	}


	public void Toggle(){
		if(state == TileState.INACTIVE) return;

		state = state == TileState.ACTIVE_A ? TileState.ACTVE_B : TileState.ACTIVE_A;
		sprite.color = state == TileState.ACTIVE_A ? Settings.global.squareColorA : Settings.global.squareColorB;

	}


	//static
	public static List<GridTile> AllSquares(){
		return TILES_LIST;

	}
		

}
