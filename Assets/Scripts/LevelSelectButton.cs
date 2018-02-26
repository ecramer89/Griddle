using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelSelectButton : MonoBehaviour {

	public int level;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnMouseDown(){

		Debug.Log("clicked");

		SceneManager.LoadScene(String.Format("Level{0}", level));

	}
}
