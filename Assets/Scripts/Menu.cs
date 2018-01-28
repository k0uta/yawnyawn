using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public int stagesCount = 5;

	public int stagesPerLine = 3;

	public GameObject buttonPrefab;

	public float xOffset = 100f;

	public float yOffset = -60f;

	AudioSource audioSource;

	public AudioClip backgroundSound;

	// Use this for initialization
	void Start () {
		InitializeStageButtons();

		transform.Find("TutorialButton").GetComponent<Button>().onClick.AddListener(OnClickTutorial);

		GameManager.SetMaxStages(stagesCount);

		InitializeBackground();
	}

	void OnClickTutorial() {
		SceneManager.LoadScene("Scenes/Menus/Tutorial");
	}

	private void InitializeStageButtons() {
		Vector3 startingPosition = transform.Find("StageButtonsStartPos").localPosition;

		for (int i = 0; i < stagesCount; i++) {
			GameObject newButton = (GameObject)GameObject.Instantiate(buttonPrefab);
			StageButton stageButton = newButton.GetComponent<StageButton>();

			stageButton.transform.SetParent(transform);
			stageButton.transform.localPosition = new Vector3(
				startingPosition.x + (i % stagesPerLine) * xOffset,
				startingPosition.y + Mathf.FloorToInt(i / stagesPerLine) * yOffset,
				startingPosition.z
			);

			stageButton.transform.localScale = Vector3.one;

			stageButton.InitStageNum(i + 1);
		}
	}

	private void InitializeBackground() {
		audioSource = GetComponent<AudioSource>();	

		audioSource.clip = backgroundSound;

		audioSource.loop = true;

		audioSource.Play();
	}
}
