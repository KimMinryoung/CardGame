using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TortureUI : MonoBehaviour {
	static TortureUI instance;
	public static TortureUI Instace {
		get { return instance; }
	}

	public static TortureManager tortureManager;
	public static GameObject Canvas;
	static GameObject SmallButton;
	List<GameObject> tortureButtons;

	// Use this for initialization
	void Awake () {
		instance = this;
		tortureButtons = new List<GameObject> ();
	}
	void Start(){
		tortureManager = TortureManager.Instance;
		SmallButton = GameManager.Instance.SmallButton;
	}
	public void CreateTortureButtons(){
		int x = -300;
		int y = 200;
		int ySpace = 50;
		foreach(var pair in tortureManager.tortures) {
			GameObject button;
			button = Util.CreateButton (SmallButton, Canvas.transform, x, y, pair.Key, () => pair.Value.tortureAction (tortureManager.prisoners ["바올리"]));
			tortureButtons.Add (button);
			y -= ySpace;
		}
	}
}
