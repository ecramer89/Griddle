using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System;

public class PlayerRecord : MonoBehaviour {

	public static PlayerRecord instance;
	private Dictionary<int, int> levelsToBestMoves;
	private int numLevels = 3;

	private int currentMoveCount;


	void Awake(){
		instance = this;
	}


	// Use this for initialization
	void Start () {
		//todo; should initialize self from data in player prefs, in the level select menu (or a pre title screen).

		levelsToBestMoves = new Dictionary<int,int>();

		//seed with 0 having a non 0 best move count to avoid special logic for initializing level 1 scene button
		levelsToBestMoves.Add(0, 1);

		for(int i=1;i<=numLevels;i++){
	
			levelsToBestMoves[i]= 0;

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


	private int GetCurrentSceneLevel(){

		int lvl;
		if(!Int32.TryParse(Regex.Match(SceneManager.GetActiveScene().name, "\\d+").Value, out lvl)) return 0;
		return lvl;
	}


	public int GetBestMoveCount(int lvl){
		int result;
		if(!levelsToBestMoves.TryGetValue(lvl, out result)) throw new ArgumentOutOfRangeException(String.Format("Invalid level passed to PlayerRecord.GetBestMoveCount: {0}", lvl));
		return result;
	
	
	}


	public bool HasBeaten(int lvl){
		return GetBestMoveCount(lvl) > 0;
	}


	public void TryUpdateBestCount(){
		int currentBestMoves;
		int lvl = GetCurrentSceneLevel();

		if(!levelsToBestMoves.TryGetValue(lvl, out currentBestMoves)) return;

		if(currentBestMoves < currentMoveCount){
			levelsToBestMoves[lvl]=currentMoveCount;
			Debug.Log(String.Format("Updated best move count: {0} now {1}", currentBestMoves, currentMoveCount));
			return;
		}


		Debug.Log(String.Format("Did not update best move count: was {0}, greater than {1}", currentBestMoves, currentMoveCount));

	}
	

}
