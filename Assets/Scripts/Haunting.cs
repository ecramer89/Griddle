using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(OscillateOpacity))]
public class Haunting : MonoBehaviour {

	private SpriteRenderer sr;
	private OscillateOpacity oo;
	[HideInInspector]
	public Vector3 center;


	public void Awake(){
		sr = GetComponent<SpriteRenderer>();
		oo = GetComponent<OscillateOpacity>();

	}




	public void Activate(){
		
		sr.gameObject.SetActive(true);
		oo.gameObject.SetActive(true);

		oo.onNewPhaseBegin+=UpdateMonster;

		UpdateMonster();
	

	}


	public void UpdateMonster(){
		//pick a random beast
		int rand = UnityEngine.Random.Range(0, Settings.global.beasts.Length); //number of demons 
		Sprite randomDemon = Settings.global.beasts[rand];
		sr.sprite = randomDemon;

		//pick a random color
		int randomColor = (int)Mathf.Round(UnityEngine.Random.Range(0, 5));
		switch(randomColor){
			case 0:
				sr.color = new Color(255, 0, 0);
			break;
			case 1:
				sr.color = new Color(255, 255, 0);
			break;
			case 2:
				sr.color = new Color(0, 255, 0);
			break;
			case 3:
				sr.color = new Color(0, 255, 255);
			break;
			case 4:
				sr.color = new Color(0, 0, 255);
			break;
			case 5:
				sr.color = new Color(255, 0, 255);
			break;
		}

		//jitter a random direction away from 'center'
		float offsetMagnitude = .5f *
			Mathf.Sqrt(Mathf.Pow(sr.sprite.bounds.extents.x, 2) + Mathf.Pow(sr.sprite.bounds.extents.y, 2));

	
		Vector3 randomDirection = 
			new Vector3(UnityEngine.Random.Range(0,1f), UnityEngine.Random.Range(0,1f), 0).normalized;



	
		transform.position = center + (randomDirection * offsetMagnitude);




	}


	public void Deactivate(){
		sr.gameObject.SetActive(false);
		oo.gameObject.SetActive(false);
	}
}
