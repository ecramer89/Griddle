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

static class DirectionMethods{

	public static Direction Opposite(this Direction direction){
		switch(direction){
		case Direction.NORTH:
			return Direction.SOUTH;
		case Direction.EAST:
			return Direction.WEST;
		case Direction.SOUTH:
			return Direction.NORTH;
		case Direction.WEST:
			return Direction.EAST;
		}

		throw new Exception(String.Format("Did you add an illegal direction? No opposite found for {0}", direction.ToString()));

	}


}

public enum TileState{
	START,
	END,
	INACTIVE,
	NULL
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
		
		if(state == TileState.NULL) return;
		if(state == TileState.INACTIVE) return;

		Game.instance.HandleMouseOverTile(this);
	
	}


	public void Rotate(){
		
		transform.Rotate(Vector3.forward * -90);

		for(int i=0;i<directions.Length;i++){
			directions[i]= (Direction)(((int)(directions[i]) + 1) % Enum.GetValues(typeof(Direction)).Length);
		}
			
	}

	public void SetState(TileState state){
		this.state = state;
		switch(this.state){
			case TileState.NULL:
				sprite.enabled = false;
			break;
			case TileState.END:
				sprite.color =  Settings.global.tileEndColor;
			break;
			case TileState.START:
				sprite.color =  Settings.global.tileStartColor;
			break;
			case TileState.INACTIVE:
				sprite.color =  Settings.global.tileInactiveColor;
			break;
		}

	}
		


	public void Toggle(){

		switch(state){
			case TileState.NULL:
			case TileState.INACTIVE:
				return;
			case TileState.START:
				SetState(TileState.END);
			break;
			case TileState.END:
				SetState(TileState.START);
			break;
		}
	}


	//static
	public static List<GridTile> All(){
		return TILES_LIST;

	}
		

}
