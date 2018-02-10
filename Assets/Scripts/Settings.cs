using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {


	public Color squareColorA;
	public Color squareColorB;


	public static Settings global;


	// Use this for initialization
	void Awake () {
		global = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
