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
		
		if(state == TileState.INACTIVE) return;

		Game.instance.HandleMouseOverTile(this);
	
	}


	public void Rotate(){
		
		transform.Rotate(Vector3.forward * -90);

		for(int i=0;i<directions.Length;i++){
			directions[i]= (Direction)(((int)(directions[i]) + 1) % Enum.GetValues(typeof(Direction)).Length);
		}
			
	}

	public void Activate(){
		state = TileState.START;
		sprite.color = Settings.global.tileStartColor;
	
	}

	public void Deactivate(){
		state = TileState.INACTIVE;
		sprite.color = Settings.global.tileInactiveColor;
	}


	public void Toggle(){
		if(state == TileState.INACTIVE) return;

		state = state == TileState.START ? TileState.END : TileState.START;
		sprite.color = state == TileState.START ? Settings.global.tileStartColor : Settings.global.tileEndColor;

	}


	//static
	public static List<GridTile> All(){
		return TILES_LIST;

	}
		

}
