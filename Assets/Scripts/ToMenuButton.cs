using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ToMenuButton : MonoBehaviour {

	void OnMouseDown(){
		SceneManager.LoadScene("LevelSelect");
	}
}
