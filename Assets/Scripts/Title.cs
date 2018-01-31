using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {
	public GameObject OnlyTextButton;
	public GameObject Canvas;

	void Awake(){
		Canvas = GameObject.FindGameObjectWithTag ("Canvas");
	}

	void Start () {
		int x = 0;
		int y = (int)(-70f * Screen.height / 720f);
		int ySpace = (int)(100f * Screen.height / 720f);
		GameObject startButton;
		startButton = Util.CreateButton (OnlyTextButton, Canvas.transform, x, y, "시작한다", () => {
			SaveData.SaveDataToFile(SaveData.NewGameData());
			SceneManager.LoadScene ("Story");
		});
		startButton.transform.Find ("Text").GetComponent<RectTransform> ().sizeDelta = new Vector2 (200, 80);
		startButton.transform.Find ("Text").GetComponent<Text> ().fontSize = 45;
		y -= ySpace;
		GameObject loadButton;
		loadButton = Util.CreateButton (OnlyTextButton, Canvas.transform, x, y, "불러온다", () => {
			SceneManager.LoadScene ("Story");
		});
		loadButton.transform.Find ("Text").GetComponent<RectTransform> ().sizeDelta = new Vector2 (200, 80);
		loadButton.transform.Find ("Text").GetComponent<Text> ().fontSize = 45;
		y -= ySpace;
		GameObject exitButton;
		exitButton = Util.CreateButton (OnlyTextButton, Canvas.transform, x, y, "종료한다", () => {
			Application.Quit();
		});
		exitButton.transform.Find ("Text").GetComponent<RectTransform> ().sizeDelta = new Vector2 (200, 80);
		exitButton.transform.Find ("Text").GetComponent<Text> ().fontSize = 45;
	}

	void Update () {
		
	}
}
