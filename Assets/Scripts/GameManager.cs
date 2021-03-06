﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static int currentStage = 1;

	public static int currentScore;

	public static int maxStages = 0;

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
		currentScore = 0;
	}

	public static void PlayCurrentStage() {
		SceneManager.LoadScene("Scenes/Stages/Stage" + currentStage);
		ResetScore();
	}

	public static void PlayNextStage() {
		currentStage += 1;

		if (currentStage > maxStages) {
			currentStage = 1;
			SceneManager.LoadScene("Scenes/Menus/Menu");
			return;
		}

		PlayCurrentStage();
	}

	public static void PlayStage(int stageNum) {
		currentStage = stageNum;
		PlayCurrentStage();
	}

	public static int GetScore() {
		return currentScore;
	}

	public static void SetMaxStages(int max) {
		maxStages = max;
	}
}
