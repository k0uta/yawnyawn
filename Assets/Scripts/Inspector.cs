using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InspectorState {
	Idle,
	Suspicious,
	Inspecting,
	Triggered
}

public class Inspector : MonoBehaviour {

	public int minIdleTurns;
	public int maxIdleTurns;
	public int minInspectingTurns;
	public int maxInspectingTurns;

	private int remainingTurns;

	public InspectorState currentState;

	private InspectorSpriteMovement inspectorSpriteMovement;

	public int turnPenalty;
	// Use this for initialization
	void Start () {
		currentState = InspectorState.Idle;
		remainingTurns = (int)Random.Range(minIdleTurns, maxIdleTurns);

		inspectorSpriteMovement = GetComponentInChildren<InspectorSpriteMovement>();
	}

	// Update is called once per frame
	void Update () {

	}

	public virtual void Move() {
		remainingTurns--;

		if (remainingTurns <= 0) {
			updateState();
		}
	}

	public void updateState() {
		if (currentState == InspectorState.Idle) {
			currentState = InspectorState.Suspicious;
			inspectorSpriteMovement.ChangeState(currentState);

			remainingTurns = 1;
		}
		else if (currentState == InspectorState.Suspicious) {
			currentState = InspectorState.Inspecting;
			inspectorSpriteMovement.ChangeState(currentState);

			remainingTurns = (int)Random.Range(minInspectingTurns, maxInspectingTurns);
		}
		else {
			currentState = InspectorState.Idle;
			inspectorSpriteMovement.ChangeState(currentState);

			remainingTurns = (int)Random.Range(minIdleTurns, maxIdleTurns);
		}
	}

	public void triggerPenalty() {
		currentState = InspectorState.Triggered;
		inspectorSpriteMovement.ChangeState(currentState);
	}
}
