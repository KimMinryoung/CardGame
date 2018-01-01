using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
		button = Util.CreateButton (SmallButton, Canvas.transform, x, y, "시작", () => SceneManager.LoadScene ("Story"));
		x += xSpace;
	}

	void Update () {
		
	}
}
