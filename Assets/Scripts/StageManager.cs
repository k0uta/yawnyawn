using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;


public enum Stage {
	Intro,
	Idle,
	Transmitting,
	Infecting
}



public class StageManager : MonoBehaviour {

	public float tickInSeconds = 1.0f;

	public float transmissionInSeconds = 1.0f;

	public float turnPenaltyInSeconds = 1.5f;

	public float infectionInSeconds = 1.0f;

	public float introInSeconds = 2.5f;

	public int transmissionTurns = 5;

	public Image introImage;

	public Text turnsText;

	public Text scoreText;

	public Clock clock;

	private Character[,] characters;

	private Inspector inspector;

	private float lastTimeUpdated = 0f;

	private Vector2Int[] directions;

	private List<Character> infectedCharacters = new List<Character>();

	private Stage currentStage = Stage.Intro;

	AudioSource audioSource;

	public AudioClip backgroundSound;


	// Use this for initialization
	void Start () {
		inspector = GameObject.FindGameObjectWithTag("Inspector").GetComponent<Inspector>();

		InitializeBackground();

		InitializeGrid();

		InitializeDirections();

		turnsText.text = transmissionTurns.ToString();
		scoreText.text = GameManager.GetScore().ToString();

		introImage.enabled = true;
	}

	// Update is called once per frame
	void Update () {
		lastTimeUpdated += Time.deltaTime;

		if (currentStage == Stage.Intro){
			if (lastTimeUpdated >= introInSeconds) {
				lastTimeUpdated = 0f;
				FinishIntro();
			}
		}
		else if (inspector.currentState == InspectorState.Triggered){
			if (lastTimeUpdated >= turnPenaltyInSeconds) {
				lastTimeUpdated = 0f;
				inspector.updateState();
			}
		}
		else if (currentStage == Stage.Transmitting) {
			if (lastTimeUpdated >= transmissionInSeconds) {
				lastTimeUpdated = 0f;
				FinishTranmissions();
			}
		}
		else if (currentStage == Stage.Idle) {
			if (lastTimeUpdated >= tickInSeconds) {
				lastTimeUpdated = 0f;
				StageTurn();
			}
		}
		else if (currentStage == Stage.Infecting) {
			if (lastTimeUpdated >= infectionInSeconds) {
				lastTimeUpdated = 0f;
				FinishInfections();
			}
		}


		// Input handling
		if (
			currentStage == Stage.Idle &&
			Input.GetButtonDown("TriggerTransmission") &&
			!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()
		) {
			lastTimeUpdated = 0f;
        	TransmissionTurn();
        }
	}

	private void FinishIntro() {
		introImage.enabled = false;
		currentStage = Stage.Idle;
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

		inspector.Move();

		clock.Tick();
	}


	private void FinishTranmissions() {
		currentStage = Stage.Infecting;

		EndTransmissions();

		if (inspector.currentState == InspectorState.Inspecting) {
			removeTransmissionTurn(inspector.turnPenalty);
			inspector.triggerPenalty();
			inspector.playSound(inspector.angrySound);

		}
		else {
			InfectCachedCharacters();
		}

		clock.Tick();
	}


	private void FinishInfections() {
		currentStage = Stage.Idle;

		EndTransmissions();

		CheckForEndGame();

		StageTurn();
	}


	private void EndTransmissions() {
		for (int i = 0; i < characters.GetLength(0); i++) {
			for (int j = 0; j < characters.GetLength(1); j++) {
				Character character = characters[i, j];

				if (character) {
					character.EndTransmission();
				}
			}
		}
	}


	private void InfectCachedCharacters() {
		foreach (Character character in infectedCharacters) {
			character.ReceiveTransmission(1);

			scoreText.text = GameManager.GetScore().ToString();
		}
	}


	private void TransmissionTurn() {
		currentStage = Stage.Transmitting;
		removeTransmissionTurn(1);

		infectedCharacters.Clear();

		for (int i = 0; i < characters.GetLength(0); i++) {
			for (int j = 0; j < characters.GetLength(1); j++) {
				CheckCharacterTransmissions(characters[i, j], new Vector2Int(i, j));
			}
		}
	}

	private void CheckCharacterTransmissions(Character character, Vector2Int position) {
		if (!character || character.currentState != CharacterState.Infected) {
			return;
		}

		character.Transmit();

		if (character.currentState != CharacterState.Transmitting) {
			return;
		}

		for (int i = 0; i < directions.Length; i++) {
			Vector2Int direction = directions[i];
			CharacterDirection directionName = GetOppositeDirection(i);
			for (int j = 1; j <= character.transmissionRange; j++) {
				Vector2Int targetPosition = position + direction * j;

				TryTransmissionToPosition(targetPosition, directionName);
			}
		}
	}


	private void TryTransmissionToPosition(Vector2Int position, CharacterDirection direction) {
		if (
			position.x < 0 || position.x >= characters.GetLength(0) ||
			position.y < 0 || position.y >= characters.GetLength(1)
		) {
			return;
		}

		Character character = characters[position.x, position.y];

		if (character && character.GetCharacterDirection() == direction) {
			infectedCharacters.Add(character);
		}
	}


	private void CheckForEndGame() {
		if (CheckWinGame())
			return;

		CheckLoseGame();
	}


	private void CheckLoseGame() {
		if (transmissionTurns <= 0) {
			SceneManager.LoadScene("Scenes/Menus/LoseGame");
		}
	}


	private bool CheckWinGame() {
		for (int i = 0; i < characters.GetLength(0); i++) {
			for (int j = 0; j < characters.GetLength(1); j++) {
				Character character = characters[i, j];

				if (character && character.currentState == CharacterState.Healthy) {
					return false;
				}
			}
		}

		SceneManager.LoadScene("Scenes/Menus/WinGame");

		return true;
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

	private void InitializeBackground() {
		audioSource = GetComponent<AudioSource>();

		audioSource.clip = backgroundSound;

		audioSource.loop = true;

		audioSource.Play();
	}

	private void removeTransmissionTurn(int num) {
		transmissionTurns -= num;
		transmissionTurns = Mathf.Max(transmissionTurns, 0);
		turnsText.text = transmissionTurns.ToString();
	}


	private CharacterDirection GetOppositeDirection(int direction) {
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
