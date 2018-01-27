using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorSpriteMovement : MonoBehaviour {

	public List<Sprite> inspectorSprites;

	private SpriteRenderer inspectorRenderer;
	private SpriteRenderer exclamationMark;

	// Use this for initialization
	void Start () {
		inspectorRenderer = GetComponent<SpriteRenderer>();
		exclamationMark = transform.Find("Exclamation").GetComponent<SpriteRenderer>();
		exclamationMark.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeState(InspectorState inspectorState) {
		Sprite newSprite = inspectorSprites[(int)inspectorState];
		inspectorRenderer.sprite = newSprite;

		exclamationMark.enabled = (inspectorState == InspectorState.Triggered);
	}
}
