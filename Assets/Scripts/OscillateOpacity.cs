using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OscillateOpacity : MonoBehaviour {


	private float timer = 0f;
	private SpriteRenderer sr;


	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, .5f * Mathf.Abs(Mathf.Sin(timer * .25f)));
		
	}
}
