using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TilePlaceholder : MonoBehaviour {

	public TileState state;

	public void Start(){
		//NULL tiles are virtually absent from the game.
		//just so that in the inspector I can mark a tile as null by disabling its sprite renderer.
		if(!GetComponent<SpriteRenderer>().enabled){
			state = TileState.NULL;
		}

	}

	public Direction[] GetDirections(){
		Sprite sprite = GetComponent<SpriteRenderer>().sprite;
	
		string[] directionsString = sprite.name.Split(new char[]{'_'});
		Direction[] directions = new Direction[directionsString.Length];
		for(int i=0;i<directionsString.Length;i++){
			directions[i] = (Direction)Enum.Parse(typeof(Direction), directionsString[i].ToUpper());
		}
		return directions;

	}
}
