using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Eye : MonoBehaviour {

	public Sprite open;
	public Sprite closed;
	private SpriteRenderer sr;


	public void Awake(){
		sr = GetComponent<SpriteRenderer>();
	}

	public void Close(){
		sr.sprite = closed;
	}


	public void Open(){
		sr.sprite=open;

	}
}
