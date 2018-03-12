using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum MouseButton{
	LEFT, RIGHT
}

public class Game : MonoBehaviour {

	public static Game instance;

	private GameObject toNextLevelButton;

	private bool gameWon = false;

	void Awake(){
		instance = this;
	}

	void Start(){
		toNextLevelButton = GameObject.Find("ToNextLevelButton");
		toNextLevelButton.SetActive(false);
	}

	public void HandleMouseOverTile(GridTile tile){
		if(gameWon) return;

		if(Input.GetMouseButtonDown(0)){
			tile.Rotate();
		}
		if(Input.GetMouseButtonDown(1)){
			ToggleReachable(tile);
			UpdateConnections();
			CheckWin();
		}


	}


	private void ToggleReachable(GridTile clicked){
		
		int totalToggled = 0;

		for(int i=0;i<clicked.directions.Length; i++){
			Direction direction = clicked.directions[i];
			switch(direction){
			case Direction.NORTH:
			case Direction.SOUTH:
				totalToggled += ToggleAdjacent(clicked,direction) ? 1 : 0;
				break;
			case Direction.EAST:
			case Direction.WEST:
				totalToggled += ToggleAdjacent(clicked,direction) ? 1 : 0;
				break;
			}

		}

		//only toggle the clicked tile if it toggled at least one other tile.
		//experimenting with removing this
		if(totalToggled > 0) {
			clicked.Toggle();
		}



	}

	public void TileGameOverAnimationDone(){
		toNextLevelButton.SetActive(true);
	}


	//returns the number of tiles that were toggled.
	private bool ToggleAdjacent(GridTile clicked, Direction direction){


		GridTile adjacent = clicked.GetAdjacentTile(direction);

		if(adjacent == null) {

			return false; 

		}

		if(adjacent.state == TileState.NULL) return false;



		//depending on current direction, if tile doesn't contain opposite direction, then break
		//since it doesn't connect




		switch(direction){
		case Direction.EAST:
			if(!adjacent.directions.AsEnumerable().Contains(Direction.WEST)) return false;
			break;
		case Direction.NORTH:
		if(!adjacent.directions.AsEnumerable().Contains(Direction.SOUTH)) return false;
			break;
		case Direction.WEST:
			if(!adjacent.directions.AsEnumerable().Contains(Direction.EAST)) return false;
			break;
		case Direction.SOUTH:
		if(!adjacent.directions.AsEnumerable().Contains(Direction.NORTH)) return false;
			break;
		}
			
		//when any tile turns off, we need to collapse ALL the connections between it and any other tile.

		adjacent.Toggle();
		return true;

	}

	private void UpdateConnections(){
		foreach(Connection connection in Connection.AllConnections()){
			UpdateConnection(connection);
		}




	}


	private void UpdateConnection(Connection connection){

		GridTile tile = connection.A;
		GridTile other = connection.GetOther(tile);

		string cname = tile.name+", "+other.name;

		if(other.state == TileState.ON && tile.state == TileState.ON){
			//draw connection

			connection.BuildConnectionFrom(tile);
			//de-activate the per tile glows; they glow together now.
			other.glow.SetActive(false);
			tile.glow.SetActive(false);
		}
		if(other.state == TileState.ON && tile.state == TileState.OFF){
			connection.ClearConnection();
			//animate a bullet heading from tile to other
			Bullet.FireBulletFromTo(tile.gameObject, other.gameObject);

		}
		if(other.state == TileState.OFF && tile.state == TileState.ON){
			connection.ClearConnection();
		
			//animate a bullet heading from tile to other
			Bullet.FireBulletFromTo(other.gameObject, tile.gameObject);


		}
		if(other.state == TileState.OFF && tile.state == TileState.OFF){
			//if there is a connection, collapse it

			connection.CollapseConnection();

		}

	}


	private void CheckWin(){
		gameWon = true;
		Debug.Log("check win?");
		foreach(GridTile tile in GridTile.All()){
			if(tile.state == TileState.OFF){
				gameWon = false;

				Debug.Log("off tile found: "+tile.name);
				break;
			}
		}

		if(gameWon){
			//start each tile's die animation.
			foreach(GridTile tile in GridTile.All()){
				tile.BeginEndGameAnimation();
			}


		}


	}
}
