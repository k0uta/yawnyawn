﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour {
	public string scenePath;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
	}

	void OnClick () {
		SceneManager.LoadScene(scenePath);
	}
}
