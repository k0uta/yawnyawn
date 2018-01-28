using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetryButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Button>().onClick.AddListener(OnClickRetry);
	}

	void OnClickRetry () {
		GameManager.PlayCurrentStage();
	}
}
