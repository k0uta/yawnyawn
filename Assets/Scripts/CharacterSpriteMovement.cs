using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterSpriteMovement : MonoBehaviour {

	public List<Sprite> healthySprites;

	public List<Sprite> infectedSprites;

	public List<Sprite> transmittingSprites;

	public List<Sprite> deadSprites;

	public SpriteRenderer yawnSprite;

	private SpriteRenderer infectedTile;

	private Dictionary<CharacterState, List<Sprite>> spriteListByState;

	private SpriteRenderer spriteRenderer;


	// Use this for initialization
	void Awake () {
		spriteListByState = new Dictionary<CharacterState, List<Sprite>>();
		spriteListByState.Add(CharacterState.Healthy, healthySprites);
		spriteListByState.Add(CharacterState.Infected, infectedSprites);
		spriteListByState.Add(CharacterState.Transmitting, transmittingSprites);
		spriteListByState.Add(CharacterState.Dead, deadSprites);

		spriteRenderer = GetComponent<SpriteRenderer>();

		infectedTile = transform.Find("InfectedTile").GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {

	}

	public void ChangeState(CharacterState characterState, CharacterDirection characterDirection, bool showYawn, int coffeeCups, int energyDrinks) {
		Sprite newSprite = spriteListByState[characterState][(int) characterDirection];
		spriteRenderer.sprite = newSprite;

		yawnSprite.enabled = showYawn;

		infectedTile.enabled = (characterState != CharacterState.Healthy);
	}
}
