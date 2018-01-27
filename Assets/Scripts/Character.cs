using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public enum CharacterDirection {
		Right,
		Left,
		Up,
		Down
	}

	public enum CharacterState {
		Healthy,
		Infected,
		Dead
	}

	public List<CharacterDirection> peekDirections = new List<CharacterDirection>();

	public int currentDirectionIndex = 0;

	public int infectionResistance = 1;

	public int transmissionRange = 1;

	public int infectionDuration = int.MaxValue;
	
	public int health = int.MaxValue;

	public CharacterState currentState = CharacterState.Healthy;



	protected int currentInfectionResistance;

	protected int currentHealth;

	protected int currentInfectionDuration;

	// Use this for initialization
	void Start() {
		currentHealth = health;
		ResetInfection();
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	public virtual void Move() {
		currentDirectionIndex++;
		if (currentDirectionIndex >= peekDirections.Count) {
			currentDirectionIndex = 0;
		}

		Debug.Log(GetCharacterDirection());
	}

	public CharacterDirection GetCharacterDirection() {
		return peekDirections[currentDirectionIndex];
	}



	public void ResetInfection() {
		currentInfectionResistance = infectionResistance;
		currentInfectionDuration = infectionDuration;
	}

	public virtual void ReceiveTransmission(int transmissionIntensity) {
		if(currentState != CharacterState.Dead) {
			currentInfectionResistance -= transmissionIntensity;
			CheckForCurrentState();
		}
	}

	public virtual void OnTransmissionTurn() {
		if(currentState == CharacterState.Infected) {
			currentInfectionDuration -= 1;
			currentHealth -= 1;

			CheckForCurrentState();
		}
	}

	protected void CheckForCurrentState() {
		if(currentHealth <= 0) {
			Die();
		}
		else if(currentInfectionDuration <= 0) {
			EndInfection();
		}
		else if(currentInfectionResistance <= 0 && currentState != CharacterState.Infected) {
			Infect();
		}
	}

	public virtual void Infect() {
		currentState = CharacterState.Infected;
	}

	public virtual void EndInfection() {
		currentState = CharacterState.Healthy;
		ResetInfection();
	}

	public virtual void Die() {
		currentState = CharacterState.Dead;
	}
}
