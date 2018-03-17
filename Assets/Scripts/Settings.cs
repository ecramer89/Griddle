using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

	public Color tileNullColor;
	public Color tileOffColor;
	public Color tileOnColor;



	public static Settings global;


	// Use this for initialization
	void Awake () {
		global = this;
	}
	

}
