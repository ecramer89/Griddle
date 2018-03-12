using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class Board : MonoBehaviour {


	private List<Column> columns;
	public static Board instance;

	void Awake(){
		instance = this;
	}
	// Use this for initialization
	void Start () {
		//find all columns, tell them to initialize.
		//establish the connections between each pir of grid tiles

		columns = new List<Column>();


		foreach(Column col in GetComponentsInChildren<Column>()){
			col.Init();
			columns.Add(col);
	
		}


		columns.Sort((colA, colB)=>colA.x - colB.x);


		for(int x = 0; x < columns.Count; x++){
			
			List<GridTile> tilesInColumn = columns[x].GetColumnTiles();

			for(int y=0;y<tilesInColumn.Count;y++){

				GridTile tile = tilesInColumn[y];


				Connection northConnection = y > 0 ? tilesInColumn[y-1].GetConnection(Direction.NORTH.Opposite()) : null;


				Connection southConnection = y < tilesInColumn.Count - 1  ? 
					(Instantiate(Resources.Load("Prefabs/Connection", typeof(GameObject))) 
						as GameObject).GetComponent<Connection>() : null;


				if(southConnection != null){
					southConnection.SetGridTile(tile);
					GridTile other = tilesInColumn[y+1];
					southConnection.SetGridTile(other);
					southConnection.transform.position = (tile.transform.position + other.transform.position) * .5f;

				}


				Connection westConnection = x > 0 ? columns[x-1].GetColumnTiles()[y].GetConnection(Direction.WEST.Opposite()) : null;


				Connection eastConnection = x < columns.Count - 1 ? 
					(Instantiate(Resources.Load("Prefabs/Connection", typeof(GameObject))) 
					as GameObject).GetComponent<Connection>() : null;


				if(eastConnection != null){
					eastConnection.SetGridTile(tile);
					GridTile other = columns[x+1].GetColumnTiles()[y];
					eastConnection.SetGridTile(other);
					eastConnection.transform.position = (tile.transform.position + other.transform.position) * .5f;

				}


			
				tile.SetConnection(Direction.SOUTH, southConnection);
				tile.SetConnection(Direction.WEST, westConnection);
				tile.SetConnection(Direction.NORTH, northConnection);
				tile.SetConnection(Direction.EAST, eastConnection);

			}
				
		}


		
	}
	
	public List<Column> GetColumns(){
		return columns;

	}
}
