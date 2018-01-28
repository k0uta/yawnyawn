using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterDirection {
	Right,
	Left,
	Up,
	Down
}

public enum CharacterState {
	Healthy,
	Infected,
	Dead,
	Transmitting
}

public class Character : MonoBehaviour {

	public int bounty = 1;

	public List<CharacterDirection> peekDirections = new List<CharacterDirection>();

	private int currentDirectionIndex = 0;

	[Range(0, 4)]
	public int infectionResistance = 1;

	public int transmissionRange = 1;

	public int infectionDuration = int.MaxValue;

	[Range(-1, 4)]
	public int health = -1;

	public CharacterState currentState = CharacterState.Healthy;



	protected int currentInfectionResistance;

	protected int currentHealth;

	protected int currentInfectionDuration;

	private CharacterSpriteMovement characterSpriteMovement;

	// Use this for initialization
	void Start() {
		currentHealth = health;
		ResetInfection();
		characterSpriteMovement = GetComponentInChildren<CharacterSpriteMovement>();


		ChangeState(currentState);
	}


	public virtual void Move() {
		currentDirectionIndex++;
		if (currentDirectionIndex >= peekDirections.Count) {
			currentDirectionIndex = 0;
		}

		ChangeState(currentState);
	}

	public CharacterDirection GetCharacterDirection() {
		return peekDirections[currentDirectionIndex];
	}



	public void ResetInfection() {
		currentInfectionResistance = infectionResistance;
		currentInfectionDuration = infectionDuration;
	}

	public virtual void ReceiveTransmission(int transmissionIntensity) {
		if (currentState == CharacterState.Healthy) {
			currentInfectionResistance -= transmissionIntensity;
			CheckForCurrentState();

			if (currentState == CharacterState.Infected) {
				ChangeState(CharacterState.Transmitting, true);

				GameManager.addScore(bounty);
			}
		}
	}

	public virtual void Transmit() {
		if(currentState == CharacterState.Infected) {
			currentInfectionDuration -= 1;
			currentHealth -= 1;
			CheckForCurrentState();

			if (currentState != CharacterState.Dead) {
				ChangeState(CharacterState.Transmitting);
			}
		}
	}

	protected void CheckForCurrentState() {
		if (currentHealth == 0 && currentState == CharacterState.Infected) {
			Die();
		}
		else if (currentInfectionDuration <= 0 && currentState == CharacterState.Infected) {
			EndInfection();
		}
		else if (
			currentInfectionResistance <= 0 &&
			currentState != CharacterState.Infected &&
			currentState != CharacterState.Dead
		) {
			Infect();
		}
	}

	public virtual void EndTransmission() {
		CheckForCurrentState();
	}

	public virtual void Infect() {
		ChangeState(CharacterState.Infected);
	}

	public virtual void EndInfection() {
		ChangeState(CharacterState.Healthy);
		ResetInfection();
	}

	public virtual void Die() {
		ChangeState(CharacterState.Dead);
	}


	public void ChangeState(CharacterState characterState, bool showYawn) {
		currentState = characterState;
		characterSpriteMovement.ChangeState(currentState, GetCharacterDirection(), showYawn, currentInfectionResistance, currentHealth);
	}


	public void ChangeState(CharacterState characterState) {
		ChangeState(characterState, false);
	}
}
