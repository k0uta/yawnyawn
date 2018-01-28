using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinGame : MonoBehaviour {

	public Text gameOverText;

	// Use this for initialization
	void Start () {
		gameOverText.text = "Game Over!\nYour Score: " + GameManager.currentScore;
		transform.Find("RetryButton").GetComponent<Button>().onClick.AddListener(OnClickRetry);
		transform.Find("MenuButton").GetComponent<Button>().onClick.AddListener(OnClickMenu);
	}

	void OnClickRetry() {
		GameManager.PlayCurrentStage();
	}

	void OnClickMenu() {
		SceneManager.LoadScene("Scenes/Menus/Menu");
	}
}
