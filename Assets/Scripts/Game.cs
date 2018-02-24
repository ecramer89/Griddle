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
			CheckWin();
		}
		//else maybe make the tile glow or something to indicate clickable

	}

	//if you did this recursively, it would function like a graph.
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
				totalToggled += ToggleAdjacent(clicked, columnTiles,direction);
				break;
			case Direction.EAST:
			case Direction.WEST:
				totalToggled += ToggleAdjacent(clicked, rowTiles,direction);
				break;
			}

		}

		//only toggle the clicked tile if it toggled at least one other tile.
		if(totalToggled > 0) {
			clicked.Toggle();
		}



	}


	private Func<int, bool> untilStart = (i) => i > - 1;

	//returns the number of tiles that were toggled.
	private int ToggleAdjacent(GridTile clicked, List<GridTile> adjacent, Direction direction){

		int advancer = direction == Direction.NORTH || direction == Direction.WEST ? -1 : 1;

		int clickedCoordinate = direction == Direction.NORTH || direction == Direction.SOUTH ? clicked.y : clicked.x;


		Func<int, bool> untilEnd = (i) => i < adjacent.Count;


		Func<int, bool> condition = advancer < 0 ? untilStart : untilEnd;

		int next = clickedCoordinate + advancer;

		int numToggled = 0;

		while(condition(next)){

			GridTile tile = adjacent[next];

			if(tile.state == TileState.INACTIVE){
				return numToggled;
			}



			//depending on current direction, if tile doesn't contain opposite direction, then break
			//since it doesn't connect
			switch(direction){
			case Direction.EAST:
				if(!tile.directions.AsEnumerable().Contains(Direction.WEST)) return numToggled;
				break;
			case Direction.NORTH:
				if(!tile.directions.AsEnumerable().Contains(Direction.SOUTH)) return numToggled;
				break;
			case Direction.WEST:
				if(!tile.directions.AsEnumerable().Contains(Direction.EAST)) return numToggled;
				break;
			case Direction.SOUTH:
				if(!tile.directions.AsEnumerable().Contains(Direction.NORTH)) return numToggled;
				break;
			}



			tile.Toggle();
			numToggled++;

			//the line cannot continue from this tile.
			if(!tile.directions.AsEnumerable().Contains(direction)) return numToggled;



			next += advancer;

		}

		return numToggled;


	}


	private void CheckWin(){
		gameWon = true;
		foreach(GridTile tile in GridTile.All()){
			if(tile.state == TileState.START){
				gameWon = false;
				break;
			}
		}

		if(gameWon){
			GameObject.Find("Text").GetComponent<Text>().enabled=true; 
		}


	}
}
