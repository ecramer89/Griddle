using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour {

	public int level;
	public GameObject bestMovesText;

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


		if(ReferenceEquals(bestMovesText, null)) {
			Debug.Log(String.Format("ERROR! Level select button with uninitialized best moves text: {0}",this.level));
			return;
		}


		this.bestMovesText.GetComponent<Text>().text = String.Format("{0}",PlayerRecord.instance.GetBestMoveCount(level));

	}
	



}
