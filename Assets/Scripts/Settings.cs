using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

	public Color tileInactiveColor;
	public Color tileStartColor;
	public Color tileEndColor;

	public string winText = "You Win!";

	public static Settings global;


	// Use this for initialization
	void Awake () {
		global = this;
	}
	

}
