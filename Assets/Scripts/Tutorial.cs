using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.Find("OKButton").GetComponent<Button>().onClick.AddListener(OnClickOk);
	}

	void OnClickOk() {
		SceneManager.LoadScene("Scenes/Menus/Menu");
	}
}
