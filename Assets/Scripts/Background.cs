using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//resize background to fill the screen viewport
		Sprite sprite = GetComponent<SpriteRenderer>().sprite;
		float spriteWidth = sprite.bounds.size.x;
		float spriteHeight = sprite.bounds.size.y;
		Vector3 newSize = transform.localScale;
		newSize.x=Screen.width / spriteWidth;
		newSize.y=Screen.height / spriteHeight;;

		transform.localScale = newSize;


	}

}
