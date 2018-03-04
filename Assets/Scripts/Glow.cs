using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour {

	private float pulseTimer = 0f;
	private Vector3 startingScale;
	private Vector3 normed;

	// Use this for initialization
	void Start () {
		startingScale = transform.localScale;
		normed = startingScale.normalized;
	}
	
	// Update is called once per frame
	void Update () {

		pulseTimer += Time.deltaTime;

		transform.localScale = startingScale + (normed * Mathf.Sin(pulseTimer));

		
	}
}
