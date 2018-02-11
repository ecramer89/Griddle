using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using System;

public class Column : MonoBehaviour {

	public int x;

	private List<GridTile> tiles = new List<GridTile>();
	private static List<Column> allColumns = new List<Column>();

	private static int gridWidth = -1;
	private static int gridHeight = -1;

	// Use this for initialization
	void Start () {
		
		int x;
	
		if(Int32.TryParse(Regex.Match(gameObject.name, "\\d+").Value, out x)){
			
			this.x = x;

		} else {
			Debug.Log("Error parsing x position from column name: "+gameObject.name);
		}
		


		TilePlaceholder[] gridspaces = GetComponentsInChildren<TilePlaceholder>();

		if(gridHeight == -1){
			gridHeight = gridspaces.Length;
		} else {
			if(gridHeight != gridspaces.Length){
				throw new Exception(String.Format("BUG! Verify that each column has equivalent ACTIVE placeholders. " +
					"Column {0} has {1} placeholders. " +
					"Set the SPRITE RENDERER of the placeholders to 'inactive' instead!", gameObject.name, gridspaces.Length));
			}
		}

		for(int i=0;i<gridspaces.Length;i++){
			TilePlaceholder placeholder = gridspaces[i];
			GridTile tile = (Instantiate(Resources.Load("Prefabs/Tile", typeof(GameObject))) as GameObject).GetComponent<GridTile>();
			tile.gameObject.transform.position = placeholder.gameObject.transform.position;
			tile.y = i;
			tile.x = this.x;
			tile.directions = placeholder.GetDirections();
			tile.column = this;
			SpriteRenderer sr = placeholder.GetComponent<SpriteRenderer>();
			tile.GetComponent<SpriteRenderer>().sprite = sr.sprite;

			if(!sr.enabled){
				tile.Deactivate();
			} else {
				tile.Activate();
			}

			tiles.Add(tile);
			Destroy(sr);
		}

		//save reference to self to columns list; update the grid width
		allColumns.Add(this);
		gridWidth = allColumns.Count;

	}


	public List<GridTile> GetColumnTiles(){
		Debug.Assert(tiles.Count == gridHeight);
		return tiles;
	}

	public static List<GridTile> GetRow(int y){

		List<GridTile> row = new List<GridTile>();

		foreach(Column col in allColumns){
			row.Add(col.tiles[y]);
		}

		Debug.Assert(row.Count == gridWidth);

		return row.OrderBy(tile => tile.x).ToList();

	}
	

}
