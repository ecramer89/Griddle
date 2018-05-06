using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System;

public class ToSceneButton : MonoBehaviour {

	public string sceneName;

	void OnMouseDown(){

		string currentScene = SceneManager.GetActiveScene().name;

		if(sceneName != null && sceneName.Trim().Length > 0) {
			SceneManager.LoadScene(sceneName);
			return;
		}

		if(Regex.IsMatch(currentScene, "Level\\d+")){
			SceneManager.LoadScene(String.Format("Level{0}", Int32.Parse(Regex.Match(currentScene, "\\d+").Value)+1));
			return;
		}


		Debug.Log(String.Format("Unknown Scene transition: from {0}",sceneName));
		SceneManager.LoadScene("LevelSelect");
	}
}
