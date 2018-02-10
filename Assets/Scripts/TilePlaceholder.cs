using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TilePlaceholder : MonoBehaviour {


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
