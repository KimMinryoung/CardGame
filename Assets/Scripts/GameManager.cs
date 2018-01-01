using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private static GameManager instance;
	public static GameManager Instance {
		get { return instance; }
	}
	public static GameObject Canvas;
	public GameObject SmallButton;
	public GameObject ImageButton;
	public GameObject StatBar;

	public static Dictionary<string, int> mainIntegers;

	public static Dictionary<string, Person> persons;
	public static Dictionary<string, Person> prisoners;

	void Awake () {
		instance = this;

		//dialogueManager = GameObject.Find ("DialogueManager").GetComponent<DialogueManager> ();
		//battleManager = GameObject.Find ("GameManager").GetComponent<BattleManager> ();

		Canvas = GameObject.FindGameObjectWithTag ("Canvas");
		TortureUI.Canvas = Canvas;
		BattleManager.Canvas = Canvas;
	}

	void Start(){
		/*
		persons = new Dictionary<string, Person> ();
		prisoners = new Dictionary<string, Person> ();

		Magic.CreateAllMagics ();

		stats ["체력"] = a;
		stats ["정신"] = b;
		stats ["민첩"] = c;
		stats ["마력"] = d;
		stats ["지능"] = e;
		stats ["행운"] = f;
		persons ["젠"] = new Person ("젠", 30, 30, 20, 20, 50, 20);
		persons ["젠"].AddMagic ("평타");
		persons ["아이리스"] = new Person ("아이리스", 15, 5, 10, 25, 70, 10);
		persons ["아이리스"].AddMagic ("강타");
		persons ["밀피"] = new Person ("밀피", 7, 70, 3, 20, 15, 100);
		persons ["밀피"].AddMagic ("평타");
		persons ["바올리"] = new Person ("바올리", 60, 50, 30, 50, 10, 80);
		persons ["바올리"].AddMagic ("강타");
		prisoners ["바올리"] = persons ["바올리"];
		//TortureManager.Instance.InitiateTorture ();
		BattleManager.Instance.InitiateBattle();
		*/
	}
}
