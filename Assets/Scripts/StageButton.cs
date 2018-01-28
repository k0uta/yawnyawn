using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class StageButton : MonoBehaviour {
	public int stageNum = 1;

	public void InitStageNum(int stage) {
		stageNum = stage;
		transform.Find("Text").GetComponent<Text>().text = stageNum.ToString();
		gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
	}

	void OnClick() {
		GameManager.PlayStage(stageNum);
	}
}
