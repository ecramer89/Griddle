using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum Direction{
	NORTH,
	EAST,
	SOUTH, 
	WEST
}

static class DirectionMethods{

	public static Direction Opposite(this Direction direction){
		switch(direction){
		case Direction.NORTH:
			return Direction.SOUTH;
		case Direction.EAST:
			return Direction.WEST;
		case Direction.SOUTH:
			return Direction.NORTH;
		case Direction.WEST:
			return Direction.EAST;
		}

		throw new Exception(String.Format("Did you add an illegal direction? No opposite found for {0}", direction.ToString()));

	}


}

public enum TileState{
	OFF,
	ON,
	NULL
}

public class GridTile : MonoBehaviour {
	
	public static List<GridTile> TILES_LIST = new List<GridTile>();

	public float rotationDurationSeconds = 1;
	private float degPerSec;

	private float rotateCounter;

	[HideInInspector]
	public Direction[] directions;
	[HideInInspector]
	private SpriteRenderer sprite;
	[HideInInspector]
	public TileState state;
	[HideInInspector]
	public int x;
	[HideInInspector]
	public int y;
	[HideInInspector]
	public Column column;

	public GameObject glow;

	private bool rotatingOut = false;


	[HideInInspector]
	public Connection[] outboundConnections;




	void Awake(){
		
		TILES_LIST.Add(this);
	
		sprite = GetComponent<SpriteRenderer>();

		outboundConnections = new Connection[4];

	
	}


	public Connection GetConnection(Direction outboundDirection){
		return outboundConnections[(int)outboundDirection];

	}

	public void SetConnection(Direction outboundDirection, Connection connection){
		if(connection != null){
			if(connection.A == null || connection.B == null || connection.A == connection.B || (connection.A != this && connection.B != this)){
				throw new Exception("Attempting to initialize a grid tile connection to a connection that does not reference this gridtile.");
			}
		}


		if(outboundConnections[(int)outboundDirection] != null){
			throw new Exception("Attempting to overwrite an existing grid tile connection.");
		}

		outboundConnections[(int)outboundDirection] = connection;
	}



	public GridTile GetAdjacentTile(Direction outboundDirection){
		Connection outboundConnection = GetConnection(outboundDirection);
		if(outboundConnection == null) return null;
		return outboundConnection.GetOther(this);

	}


	public void RotateDown(){
		rotatingOut = true;

	}


	public void BeginEndGameAnimation(){
		if(state == TileState.NULL) {
			GameObject.Destroy(glow);
			GameObject.Destroy(gameObject);
		}
		glow.GetComponent<Glow>().FadeOut(RotateDown);

	}

	//use on mouse over instead of onMouseDown since former only invoked if user presses left mouse button
	public void OnMouseOver(){

		if(rotateCounter > 0) return; //don't allow other effects if we are rotating.
		
		if(state == TileState.NULL) return;
	
	

		Game.instance.HandleMouseOverTile(this);
	
	}

	public void Update(){


		if(rotatingOut){
			transform.Rotate(Vector3.forward * Time.deltaTime * 1.5f);
			transform.localScale = transform.localScale * .99f;

			if(transform.localScale.magnitude < .1){

				rotatingOut = false;
				GameObject.Destroy(gameObject);
				Game.instance.TileGameOverAnimationDone();
			
			}

		} else {
		//rotate 90 degrees over course of a second;
		//since variable frames elapse per second
		//multiply delta time/seconds by amount of frames elapsed
		//track the total degrees we've rotated so we know when to stop.

			float rotateDegreesThisFrame = 90 * Time.deltaTime;

			if(rotateCounter > 0){
				rotateCounter -= rotateDegreesThisFrame;
				transform.Rotate(Vector3.forward * -rotateDegreesThisFrame);
			}

		}




	}


	public void Rotate(){


	
		rotateCounter = 90f;


		for(int i=0;i<directions.Length;i++){
			directions[i]= (Direction)(((int)(directions[i]) + 1) % Enum.GetValues(typeof(Direction)).Length);
		}
			
	}

	public void SetState(TileState state){
		this.state = state;
		switch(this.state){
			case TileState.NULL:
				sprite.enabled = false;
				glow.SetActive(false);
			break;
			case TileState.ON:
				sprite.color =  Settings.global.tileEndColor;
				glow.SetActive(true);
			    
				Glow.Synchronize();
			break;
			case TileState.OFF:
				sprite.color =  Settings.global.tileStartColor;
				glow.SetActive(false);
			break;
	
	
		}

	}
		


	public void Toggle(){

		switch(state){
			case TileState.NULL:
				return;
			case TileState.OFF:
				SetState(TileState.ON);
			break;
			case TileState.ON:
				SetState(TileState.OFF);
			break;
		}
	}


	public void Affect(GridTile other){
		
		other.SetState(this.state);
	
	}


	//static
	public static List<GridTile> All(){
		return TILES_LIST;

	}
		

	public void OnDestroy(){
		TILES_LIST.Remove(this);
	}

}
