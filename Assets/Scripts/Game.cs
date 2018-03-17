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

		Board.instance.Init();

		DrawConnections();
		UpdateConnections();

	 
	}

	public void HandleMouseOverTile(GridTile tile){
		if(gameWon) return;

		if(Input.GetMouseButtonDown(0)){
			tile.Rotate();
		}
		if(Input.GetMouseButtonDown(1)){
			ToggleReachable(tile);
			UpdateConnections(tile);
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

	private void DrawConnections(){
		foreach(Connection connection in Connection.AllConnections()){
			connection.BuildConnectionFrom(connection.A);
		}

	}

	private void UpdateConnections(GridTile clicked = null){
		foreach(Connection connection in Connection.AllConnections()){
			UpdateConnection(connection, clicked);
		}




	}


	private void UpdateConnection(Connection connection, GridTile clicked){

		GridTile tile = connection.A;
		GridTile other = connection.GetOther(tile);


		connection.SetColor(Settings.global.tileOffColor);

		if(other.state == TileState.ON && tile.state == TileState.ON){
			//draw connection
			if(tile == clicked || other == clicked) {
				connection.SetColor(Settings.global.tileOnColor);
			}
			//connection.BuildConnectionFrom(tile);
			//de-activate the per tile glows; they glow together now.
			//other.glow.SetActive(false);
			//tile.glow.SetActive(false);
		}
		if(other.state == TileState.ON && tile.state == TileState.OFF){
			
			//connection.ClearConnection();
			//if user clicked on the tile, animate a bullet heading from tile to other
			if(tile == clicked || other == clicked) {
				Bullet.FireBulletFromTo(tile.gameObject, other.gameObject);
			}

		}
		if(other.state == TileState.OFF && tile.state == TileState.ON){
			//connection.ClearConnection();
		
			//animate a bullet heading from tile to other
			if(other == clicked || tile == clicked){
				Bullet.FireBulletFromTo(other.gameObject, tile.gameObject);
			}




		}
		if(other.state == TileState.OFF && tile.state == TileState.OFF){
			//if there is a connection, collapse it

			//connection.CollapseConnection();

			connection.SetColor(Settings.global.tileOffColor);

		}

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
			//start each tile's die animation.
			foreach(GridTile tile in GridTile.All()){
				tile.BeginEndGameAnimation();
			}




		}


	}
}
