using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using System;


public class StageManager : MonoBehaviour {

	public float tickInSeconds = 1.0f;

	public int transmissionTurns = 5;

	private Character[,] characters;

	private float lastTimeUpdated = 0f;

	private Vector2Int[] directions;

	// Use this for initialization
	void Start () {
		InitializeGrid();

		InitializeDirections();
	}

	// Update is called once per frame
	void Update () {
		lastTimeUpdated += Time.deltaTime;

		if (lastTimeUpdated >= tickInSeconds) {
			lastTimeUpdated = 0f;
			StageTurn();
		}


		// Input handling
		if (Input.GetButtonDown("TriggerTransmission"))
        {
        	TransmissionTurn();
        }
	}

	private void StageTurn() {
		for (int i = 0; i < characters.GetLength(0); i++) {
			for (int j = 0; j < characters.GetLength(1); j++) {
				Character character = characters[i, j];
				if (character) {
					character.Move();
				}
			}
		}
	}


	private void TransmissionTurn() {
		for (int i = 0; i < characters.GetLength(0); i++) {
			for (int j = 0; j < characters.GetLength(1); j++) {
				CheckCharacterTranssions(characters[i, j], new Vector2Int(i, j));
			}
		}
	}

	private void CheckCharacterTranssions(Character character, Vector2Int position) {
		if (!character || character.currentState != CharacterState.Infected) {
			return;
		}

		character.Transmit();

		Vector2Int direction = directions[(int)character.GetCharacterDirection()];

		for (int i = 1; i <= character.transmissionRange; i++) {
			position += direction;

			tryTransmissionToPosition(position);
		}
	}

	private void tryTransmissionToPosition(Vector2Int position) {
		Character character = characters[position.x, position.y];

		if (character) {
			character.ReceiveTransmission(1);
		}
	}


	private void CheckForEndGame() {

	}


	private void TriggerLoseGame() {

	}


	private void TriggerWinGame() {

	}


	private void InitializeGrid() {
		GameObject[] characterObjects = GameObject.FindGameObjectsWithTag("Character");

		int minCol = int.MaxValue;
		int minRow = int.MaxValue;
		int maxRow = int.MinValue;
		int maxCol = int.MinValue;

		float tileSize = 2.25f;

		foreach (GameObject characterObject in characterObjects) {
			IsoTransform isoTransform = characterObject.GetComponent<IsoTransform>();
			Vector2Int gridPosition = new Vector2Int(
				Mathf.FloorToInt(isoTransform.Position.x / tileSize),
				Mathf.FloorToInt(isoTransform.Position.z / tileSize));
			minCol = Mathf.Min(minCol, gridPosition.x);
			maxCol = Mathf.Max(maxCol, gridPosition.x);

			minRow = Mathf.Min(minRow, gridPosition.y);
			maxRow = Mathf.Max(maxRow, gridPosition.y);

		}


		int stageRows = maxRow - minRow + 1;
		int stageCols = maxCol - minCol + 1;

		characters = new Character[stageRows, stageCols];

		Vector2Int gridOffset = new Vector2Int(-minCol, -minRow);

		foreach (GameObject characterObject in characterObjects) {
			Character character = characterObject.GetComponent<Character>();
			IsoTransform isoTransform = characterObject.GetComponent<IsoTransform>();
			Vector2Int gridPosition = new Vector2Int(
				Mathf.FloorToInt(isoTransform.Position.x / tileSize),
				Mathf.FloorToInt(isoTransform.Position.z / tileSize)
			);
			gridPosition += gridOffset;

			characters[stageRows - gridPosition.y - 1, gridPosition.x] = character;
		}
	}

	private void InitializeDirections() {
		directions = new Vector2Int[4];

		directions[(int)CharacterDirection.Up] = new Vector2Int(1, 0);
		directions[(int)CharacterDirection.Down] = new Vector2Int(-1, 0);
		directions[(int)CharacterDirection.Right] = new Vector2Int(0, 1);
		directions[(int)CharacterDirection.Left] = new Vector2Int(0, -1);
	}
}
