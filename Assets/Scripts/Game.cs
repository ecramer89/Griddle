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

		//fire bullet between clicked and adjacent.
		//state of clicked tile won't update until after loop finishes (since toggling state of clicked conditonal
		//on toggling state of at least one other grid tile) so we just preview the next state here.
		//(i.e., we know that clicked will toggle state, since if we're here then clicked toggled something)
		TileState clickedTileNextState = clicked.state == TileState.OFF ? TileState.ON : TileState.OFF;

		if(adjacent.state == TileState.ON && clickedTileNextState == TileState.OFF){
			Bullet.FireBulletFromTo(clicked.gameObject, adjacent.gameObject);
		}
		if(adjacent.state == TileState.OFF && clickedTileNextState == TileState.ON){
			Bullet.FireBulletFromTo(adjacent.gameObject, clicked.gameObject);
		}





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

			connection.SetColor(Settings.global.tileOnColor);

			if(tile == clicked) {
				Bullet.FireBulletFromTo(tile.gameObject, other.gameObject);
			}

			if(other == clicked){
				Bullet.FireBulletFromTo(other.gameObject, tile.gameObject);
			}
		
		}

		if(other.state == TileState.OFF && tile.state == TileState.OFF){
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
