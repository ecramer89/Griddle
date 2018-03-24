using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SizePulse : MonoBehaviour {

	private float pulseTimer = 0f;
	private Vector3 startingScale;


	private bool fading = false;
	private Action onFaded;


	private static List<SizePulse> allSizePulses = new List<SizePulse>();


	// Use this for initialization
	void Start () {
		startingScale = transform.localScale;

		allSizePulses.Add(this);
	}

	public void UpateScale(Vector3 newScale){
		this.gameObject.transform.localScale = newScale;
		startingScale = newScale;

	}
	
	// Update is called once per frame
	void Update () {

		if(fading){
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			float alpha = sr.color.a;
			alpha -= Time.deltaTime;
			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

			if(alpha <= 0){
				onFaded();
				GameObject.Destroy(gameObject);
			}



		} else {

			pulseTimer += Time.deltaTime;

			transform.localScale = startingScale + (startingScale * Mathf.Sin(pulseTimer * .5f)) / 2.5f;

		}

		
	}

	public void OnDestory(){
		allSizePulses.Remove(this);
	}


	public static void Synchronize(){
		foreach(SizePulse sizePulse in allSizePulses){
			sizePulse.pulseTimer = 0;
		}

	}


	public void FadeOut(Action onFaded){
		fading = true;
		this.onFaded = onFaded;

	}



}
