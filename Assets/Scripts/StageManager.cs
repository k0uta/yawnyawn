using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using System;


public class StageManager : MonoBehaviour {

	public float tickInSeconds = 2.0f;

	public float transmissionInSeconds = 4.0f;

	public int transmissionTurns = 5;

	private bool transmitting = false;

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

		if (transmitting) {
			if (lastTimeUpdated >= transmissionInSeconds) {
				lastTimeUpdated = 0f;
				FinishTranmissions();
			}
		}
		else {
			if (lastTimeUpdated >= tickInSeconds) {
				lastTimeUpdated = 0f;
				StageTurn();
			}
		}


		// Input handling
		if (!transmitting && Input.GetButtonDown("TriggerTransmission"))
        {
			lastTimeUpdated = 0f;
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


	private void FinishTranmissions() {
		transmitting = false;

		for (int i = 0; i < characters.GetLength(0); i++) {
			for (int j = 0; j < characters.GetLength(1); j++) {
				Character character = characters[i, j];

				if (character) {
					character.EndTransmission();
				}
			}
		}

		StageTurn();
	}


	private void TransmissionTurn() {
		transmissionTurns -= 1;
		transmitting = true;

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

		// Vector2Int direction = directions[(int)character.GetCharacterDirection()];

		for (int i = 0; i < directions.Length; i++) {
			Vector2Int direction = directions[i];
			CharacterDirection directionName = getOppositeDirection(i);

			for (int j = 1; j <= character.transmissionRange; j++) {
				Vector2Int targetPosition = position + direction * j;

				tryTransmissionToPosition(targetPosition, directionName);
			}
		}
	}


	private void tryTransmissionToPosition(Vector2Int position, CharacterDirection direction) {
		if (
			position.x < 0 || position.x >= characters.GetLength(0) ||
			position.y < 0 || position.y >= characters.GetLength(1)
		) {
			return;
		}

		Character character = characters[position.x, position.y];

		if (character && character.GetCharacterDirection() == direction) {
			character.ReceiveTransmission(1);
		}
	}


	private void CheckForEndGame() {
		if (transmissionTurns <= 0) {
			TriggerLoseGame();
		}
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

		directions[(int)CharacterDirection.Right] = new Vector2Int(0, 1);
		directions[(int)CharacterDirection.Left] = new Vector2Int(0, -1);
		directions[(int)CharacterDirection.Up] = new Vector2Int(-1, 0);
		directions[(int)CharacterDirection.Down] = new Vector2Int(1, 0);
	}


	private CharacterDirection getOppositeDirection(int direction) {
		if ((int)CharacterDirection.Right == direction) {
			return CharacterDirection.Left;
		}
		else if ((int)CharacterDirection.Left == direction) {
			return CharacterDirection.Right;
		}
		else if ((int)CharacterDirection.Up == direction) {
			return CharacterDirection.Down;
		}
		else if ((int)CharacterDirection.Down == direction) {
			return CharacterDirection.Up;
		}

		return CharacterDirection.Up;
	}
}
