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
	public void Init () {
		
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
			tile.gameObject.name=x+""+i;
			tile.gameObject.transform.position = placeholder.gameObject.transform.position;
			tile.y = i;
			tile.x = this.x;
			GameObject glow = (Instantiate(Resources.Load("Prefabs/Glow", typeof(GameObject))) 
				as GameObject);
			glow.transform.position=tile.transform.position;
			tile.glow = glow;
			tile.directions = placeholder.GetDirections();

			Eye eye = (Instantiate(Resources.Load("Prefabs/Eye", typeof(GameObject))) 
				as GameObject).GetComponent<Eye>();
			eye.transform.position=tile.transform.position;
			eye.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
			tile.eye = eye;

		
			tile.column = this;
			SpriteRenderer sr = placeholder.GetComponent<SpriteRenderer>();
			tile.GetComponent<SpriteRenderer>().sprite = sr.sprite;
			tile.SetState(placeholder.state);
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
