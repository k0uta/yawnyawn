using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoseGame : MonoBehaviour {

	public Text gameOverText;

	// Use this for initialization
	void Start () {
		gameOverText.text = "Game Over!\nYour Score: " + GameManager.currentScore;
		transform.Find("RetryButton").GetComponent<Button>().onClick.AddListener(OnClickRetry);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnClickRetry() {
		GameManager.PlayCurrentStage();
	}
}
