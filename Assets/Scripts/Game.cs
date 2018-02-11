using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
		}
		//else maybe make the tile glow or something to indicate clickable

	}

	//if you did this recursively, it would function like a graph.
	private void ToggleReachable(GridTile clicked){



		Column column = clicked.column;
	
		List<GridTile> columnTiles = column.GetColumnTiles();
		List<GridTile> rowTiles = Column.GetRow(clicked.y);



		for(int i=0;i<clicked.directions.Length; i++){
			
			switch(clicked.directions[i]){
				case Direction.NORTH:

					ToggleAdjacent(clicked, columnTiles,clicked.y, -1);
					break;
				case Direction.SOUTH:
					ToggleAdjacent(clicked, columnTiles,clicked.y, 1);
					break;
				case Direction.EAST:
					ToggleAdjacent(clicked, rowTiles,clicked.x, 1);
					break;
				case Direction.WEST:
					ToggleAdjacent(clicked, rowTiles,clicked.x, -1);
					break;

			}

		}



	}
		

	private Func<int, bool> untilStart = (i) => i > - 1;

	private void ToggleAdjacent(GridTile clicked, List<GridTile> adjacent, int clickedCoordinate, int direction){
		
		Func<int, bool> untilEnd = (i) => i < adjacent.Count;

		int advancer = direction/(Math.Abs(direction));

		Func<int, bool> condition = advancer < 0 ? untilStart : untilEnd;

		int start = clickedCoordinate;


		while(condition(start)){

			GridTile tile = adjacent[start];

			if(tile.state == TileState.INACTIVE){
				return;
			}


			tile.Toggle();


			start += advancer;

	
		}

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
			GameObject.Find("Text").SetActive(true);
		}


	}
}
