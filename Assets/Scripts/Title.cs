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
		int y = -100;
		int ySpace = 100;
		GameObject button;
		button = Util.CreateButton (OnlyTextButton, Canvas.transform, x, y, "시작한다", () => SceneManager.LoadScene ("Story"));
		button.transform.Find ("Text").GetComponent<RectTransform> ().sizeDelta = new Vector2 (200, 80);
		button.transform.Find ("Text").GetComponent<Text> ().fontSize = 45;
		y += ySpace;
	}

	void Update () {
		
	}
}
