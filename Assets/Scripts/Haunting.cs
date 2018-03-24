using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(OscillateOpacity))]
public class Haunting : MonoBehaviour {

	private SpriteRenderer sr;
	private OscillateOpacity oo;
	private Vector3 startingPosition;

	public void Awake(){
		sr = GetComponent<SpriteRenderer>();
		oo = GetComponent<OscillateOpacity>();
		startingPosition = transform.position;
	}

	public void Activate(){
		sr.gameObject.SetActive(true);
		oo.gameObject.SetActive(true);
		int rand = UnityEngine.Random.Range(0, Settings.global.beasts.Length); //number of demons 
		Sprite randomDemon = Settings.global.beasts[rand];
		sr.sprite = randomDemon;
		sr.color = new Color(255, 0, 0);
		/*
		 * new Vector3(
				haunting.GetComponent<SpriteRenderer>().size.x/2, haunting.GetComponent<SpriteRenderer>().size.y/2,0);
		 * 
		 * 
		 * */
		rand = UnityEngine.Random.Range(0,4);
		switch(rand){
			case 1:
			
			break;
			case 2:
			
			break;
			case 3:
			
			break;
			case 4:
			
			break;


		}

	}


	public void Deactivate(){
		sr.gameObject.SetActive(false);
		oo.gameObject.SetActive(false);
	}
}
