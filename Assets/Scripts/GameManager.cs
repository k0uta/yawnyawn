using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static int currentStage = 1;

	public static float currentScore;

	public static int totalStages = 5;

	// Use this for initialization
	void Start () {
		ResetScore();
	}

	// Update is called once per frame
	void Update () {

	}

	public static void addScore(int num) {
		currentScore += num;
	}

	public static void ResetScore() {
		currentScore = 0f;
	}

	public static void PlayCurrentStage() {
		SceneManager.LoadScene("Scenes/Stages/Stage" + currentStage);
		ResetScore();
	}
}
