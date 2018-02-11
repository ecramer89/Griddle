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
	private Text text;

	void Awake(){
		instance = this;
	}

	void Start(){
		text = GameObject.Find("Text").GetComponent<Text>();
	}

	public void HandleMouseOverTile(GridTile tile){
		if(gameWon) return;

		if(Input.GetMouseButtonDown(0)){
			tile.Rotate();
		}

		if(Input.GetMouseButtonDown(1)){
			
			int totalToggled = ToggleReachable(tile, tile, 0, new HashSet<GridTile>());

			if(totalToggled > 0){
				tile.Toggle();
			}

			CheckWin();
		}
		//else maybe make the tile glow or something to indicate clickable

	}


	private int ToggleReachable(GridTile clicked, GridTile from, int numTilesToggled, HashSet<GridTile> visited){

		visited.Add(from);

		Column column = from.column;
	
		List<GridTile> columnTiles = column.GetColumnTiles();
		List<GridTile> rowTiles = Column.GetRow(from.y);


		for(int i=0;i<from.directions.Length; i++){
			Direction direction = from.directions[i];
			switch(direction){
				case Direction.NORTH:
			    case Direction.SOUTH:
				numTilesToggled += ToggleNext(clicked, from, columnTiles,direction, numTilesToggled, visited);
					break;
				case Direction.EAST:
				case Direction.WEST:
				numTilesToggled += ToggleNext(clicked, from, rowTiles,direction, numTilesToggled, visited);
					break;
			}

		}

		return numTilesToggled;



	}
		

	private Func<int, bool> untilStart = (i) => i > - 1;

	//returns the number of tiles that were toggled.
	private int ToggleNext(GridTile clicked, GridTile from, List<GridTile> adjacent, Direction direction, int numTilesToggled, HashSet<GridTile> visited){
		
		int advancer = direction == Direction.NORTH || direction == Direction.WEST ? -1 : 1;

		int clickedCoordinate = direction == Direction.NORTH || direction == Direction.SOUTH ? from.y : from.x;


		Func<int, bool> untilEnd = (i) => i < adjacent.Count;


		Func<int, bool> condition = advancer < 0 ? untilStart : untilEnd;

		int next = clickedCoordinate + advancer;

		if(!condition(next)) return numTilesToggled;


		GridTile nextTile = adjacent[next];

		if(nextTile.state == TileState.NULL) return numTilesToggled;

		if(visited.Contains(nextTile)) return numTilesToggled;

		//depending on current direction, if tile doesn't contain opposite direction, then break
		//since it doesn't connect
		if(!nextTile.directions.AsEnumerable().Contains(direction.Opposite())) return numTilesToggled;
	
		if(nextTile.state == TileState.INACTIVE) {
			if(from == clicked){
				nextTile.SetState(TileState.START);
			}
			else return numTilesToggled;
		}

		nextTile.Toggle();

	
	    return ToggleReachable(clicked, nextTile, ++numTilesToggled, visited);


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
			text.text = Settings.global.winText;
			text.enabled=true; 
		}


	}
}
