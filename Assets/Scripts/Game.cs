using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
public enum MouseButton{
	LEFT, RIGHT
}

public class Game : MonoBehaviour {

	public static Game instance;

	private bool gameWon = false;

	void Awake(){
		instance = this;
	}

	public void HandleMouseOverTile(GridTile tile){
		if(gameWon) return;

		if(Input.GetMouseButtonDown(0)){
			tile.Rotate();
		}
		if(Input.GetMouseButtonDown(1)){
			ToggleReachable(tile);
			//CheckWin();
		}
		//else maybe make the tile glow or something to indicate clickable

	}


	private void ToggleReachable(GridTile clicked){

		Column column = clicked.column;

		List<GridTile> columnTiles = column.GetColumnTiles();
		List<GridTile> rowTiles = Column.GetRow(clicked.y);


		int totalToggled = 0;

		for(int i=0;i<clicked.directions.Length; i++){
			Direction direction = clicked.directions[i];
			switch(direction){
			case Direction.NORTH:
			case Direction.SOUTH:
				totalToggled += ToggleAdjacent(clicked, columnTiles,direction) ? 1 : 0;
				break;
			case Direction.EAST:
			case Direction.WEST:
				totalToggled += ToggleAdjacent(clicked, rowTiles,direction) ? 1 : 0;
				break;
			}

		}

		//only toggle the clicked tile if it toggled at least one other tile.
		//experimenting with removing this
		if(totalToggled > 0) {
			clicked.Toggle();
		}



	}


	private Func<int, bool> untilStart = (i) => i > - 1;

	//returns the number of tiles that were toggled.
	private bool ToggleAdjacent(GridTile clicked, List<GridTile> adjacent, Direction direction){

		int advancer = direction == Direction.NORTH || direction == Direction.WEST ? -1 : 1;

		int clickedCoordinate = direction == Direction.NORTH || direction == Direction.SOUTH ? clicked.y : clicked.x;


		Func<int, bool> untilEnd = (i) => i < adjacent.Count;


		Func<int, bool> condition = advancer < 0 ? untilStart : untilEnd;

		int adjacentIndex = clickedCoordinate + advancer;

		if(!condition(adjacentIndex)) return false;

	
		GridTile tile = adjacent[adjacentIndex];

		if(tile.state == TileState.NULL) return false;



		//depending on current direction, if tile doesn't contain opposite direction, then break
		//since it doesn't connect
		switch(direction){
		case Direction.EAST:
		if(!tile.directions.AsEnumerable().Contains(Direction.WEST)) return false;
			break;
		case Direction.NORTH:
		if(!tile.directions.AsEnumerable().Contains(Direction.SOUTH)) return false;
			break;
		case Direction.WEST:
		if(!tile.directions.AsEnumerable().Contains(Direction.EAST)) return false;
			break;
		case Direction.SOUTH:
		if(!tile.directions.AsEnumerable().Contains(Direction.NORTH)) return false;
			break;
		}


		//shoot a bullet at tile from clicked
		Bullet bullet = (Instantiate(Resources.Load("Prefabs/Bullet", typeof(GameObject))) 
			as GameObject).GetComponent<Bullet>();

		//shoot light from clicked to adjacent tile if toggling adjacent tile on
		if(tile.state == TileState.OFF){
			bullet.transform.position = clicked.transform.position;
			bullet.SetTarget(tile.gameObject);
		} else {
		//absorb light from target back into clicked if toggling adjacent off
			bullet.transform.position = tile.gameObject.transform.position;
			bullet.SetTarget(clicked.gameObject);
		}

		tile.Toggle();
		return true;

	}


	private void CheckWin(){
		gameWon = true;
		foreach(GridTile tile in GridTile.All()){
			if(tile.state == TileState.OFF){
				gameWon = false;
				break;
			}
		}

		if(gameWon){
			GameObject.Find("Text").GetComponent<Text>().enabled=true; 
		}


	}
}
