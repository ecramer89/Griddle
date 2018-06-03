using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelSelectButton : MonoBehaviour {

	public int level;

	// Use this for initialization
	void Start () {
		
		//disable buttons for levels that player can't access
		this.enabled = PlayerRecord.instance.HasBeaten(level-1);
		Debug.Log(this.enabled+" level enabled "+level);
		if(!this.enabled){
			Color c = GetComponent<SpriteRenderer>().color;
			Color dark = new Color(c.r/2, c.g/2, c.b/2);
			GetComponent<SpriteRenderer>().color = dark;

		}
	}
	



}
