using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorSpriteMovement : MonoBehaviour {

	public List<Sprite> inspectorSprites;

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeState(InspectorState inspectorState) {
		Sprite newSprite = inspectorSprites[(int)inspectorState];
		spriteRenderer.sprite = newSprite;
	}
}
