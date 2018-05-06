using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System;

public class PlayerRecord : MonoBehaviour {

	public static PlayerRecord instance;
	private Dictionary<string, int> levelsToBestMoves;

	private int currentMoveCount;


	void Awake(){
		instance = this;
	}
	// Use this for initialization
	void Start () {
		//todo; should initialize self from data in player prefs, in the level select menu (or a pre title screen).

		levelsToBestMoves = new Dictionary<string,int>();

		for(int i=0;i<SceneManager.sceneCount;i++){
			Scene scene = SceneManager.GetSceneAt(i);
			if(Regex.IsMatch(scene.name, "Level\\d+")){
				levelsToBestMoves[scene.name]= 0;
			}
		}


		GameObject.DontDestroyOnLoad(gameObject);

		SceneManager.sceneLoaded+=ResetMoveCounter;
	}


	public void ResetMoveCounter(Scene next, LoadSceneMode mode){
		currentMoveCount = 0;
	}


	public void LogMove(){
		currentMoveCount ++;
	}


	public void TryUpdateBestCount(){
		int currentBestMoves;
		String currentLevel = SceneManager.GetActiveScene().name;
		if(!levelsToBestMoves.TryGetValue(currentLevel, out currentBestMoves)) return;

		if(currentBestMoves < currentMoveCount){
			levelsToBestMoves[currentLevel]=currentMoveCount;
			Debug.Log(String.Format("Updated best move count: {0} now {1}", currentBestMoves, currentMoveCount));
			return;
		}


		Debug.Log(String.Format("Did not update best move count: was {0}, greater than {1}", currentBestMoves, currentMoveCount));

	}
	

}
