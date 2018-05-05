using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(SpriteRenderer))]
public class OscillateOpacity : MonoBehaviour {


	private float timer = 0f;
	private float damp = .25f;
	private float scale = .35f;


	private SpriteRenderer sr;
	public event Action onNewPhaseBegin = ()=>{};


	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		float newAlpha = scale * Mathf.Abs(Mathf.Sin(timer * damp));


		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, newAlpha);


		if(newAlpha == 0){
			onNewPhaseBegin();
		}
		
	}
}
