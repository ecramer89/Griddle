using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Game : MonoBehaviour {

	public static Game instance;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void MakeMove(GridTile clicked){
		Debug.Log("tile clicked");
		clicked.Rotate();

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
		
	private Func<int, int> forward = (i) => ++i;
	private Func<int, int> backward = (i) => --i;
	private Func<int, bool> untilStart = (i) => i > - 1;

	private void ToggleAdjacent(GridTile clicked, List<GridTile> adjacent, int start, int direction){
		
		Func<int, bool> untilEnd = (i) => i < adjacent.Count;

		Func<int, bool> condition = direction < 0 ? untilStart : untilEnd;
		Func<int,int> advance = direction < 0 ? backward: forward;

	
		while(condition(start)){

			GridTile tile = adjacent[start];

			if(tile.state == TileState.INACTIVE){
				return;
			}


			tile.Toggle();


			start = advance(start);

	
		}

	}
}
