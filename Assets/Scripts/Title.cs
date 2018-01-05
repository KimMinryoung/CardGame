using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {
	public GameObject SmallButton;
	public GameObject Canvas;

	void Awake(){
		Canvas = GameObject.FindGameObjectWithTag ("Canvas");
	}

	void Start () {
		int x = 0;
		int y = 0;
		int xSpace = 100;
		GameObject button;
		button = Util.CreateButton (SmallButton, Canvas.transform, x, y, "시작한다", () => SceneManager.LoadScene ("Story"));
		button.GetComponent<RectTransform> ().sizeDelta = new Vector2 (300, 150);
		button.GetComponent<Image> ().color = new Color (1.0f, 1.0f, 1.0f, 0.5f);
		button.transform.Find ("Text").GetComponent<RectTransform> ().sizeDelta = new Vector2 (300, 150);
		button.transform.Find ("Text").GetComponent<Text> ().fontSize = 45;
		x += xSpace;
	}

	void Update () {
		
	}
}
