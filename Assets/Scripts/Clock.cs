using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour {

	public List<Sprite> sprites;

	private SpriteRenderer spriteRenderer;

	private int currentSpriteIndex = 0;

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}


	public void Tick() {
		currentSpriteIndex += 1;
		currentSpriteIndex %= sprites.Count;

		spriteRenderer.sprite = sprites[currentSpriteIndex];
	}
}
