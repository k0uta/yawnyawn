using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Menu : MonoBehaviour {

	public int stagesCount = 5;

	public GameObject buttonPrefab;

	public float xOffset = 100f;

	public float yOffset = -60f;


	// Use this for initialization
	void Start () {
		InitializeStageButtons();
	}


	private void InitializeStageButtons() {
		Vector3 startingPosition = transform.Find("StageButtonsStartPos").position;

		for (int i = 0; i < stagesCount; i++) {
			GameObject newButton = (GameObject)GameObject.Instantiate(buttonPrefab);
			StageButton stageButton = newButton.GetComponent<StageButton>();

			stageButton.transform.SetParent(transform);
			stageButton.transform.position = startingPosition;
			stageButton.transform.position = new Vector3(
				startingPosition.x + (i % 2) * xOffset,
				startingPosition.y + Mathf.FloorToInt(i / 2) * yOffset,
				startingPosition.z
			);

			stageButton.InitStageNum(i + 1);
		}
	}
}
