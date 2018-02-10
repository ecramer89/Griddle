using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Column : MonoBehaviour {

	public int x;

	private List<GridTile> tiles = new List<GridTile>();
	private static List<Column> allColumns = new List<Column>();

	private static GridTile[][] fullGrid;

	// Use this for initialization
	void Start () {
		
		allColumns.Add(this);

		TilePlaceholder[] gridspaces = GetComponentsInChildren<TilePlaceholder>();

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
			tiles.Add(tile);
			Destroy(sr);
		}
	}


	public List<GridTile> GetColumnTiles(){
		return tiles;
	}

	public static List<GridTile> GetRow(int y){

		List<GridTile> row = new List<GridTile>();

		foreach(Column col in allColumns){
			row.Add(col.tiles[y]);
		}

		return row.OrderBy(tile => tile.x).ToList();

	}
	

}
