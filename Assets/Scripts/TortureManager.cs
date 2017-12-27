using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TortureManager : MonoBehaviour {
	private static TortureManager instance;
	public static TortureManager Instance {
		get { return instance; }
	}

	public static TortureUI tortureUI;

	public Dictionary<string, Torture> tortures;
	public Dictionary<string, Person> prisoners;

	void Awake(){
		instance = this;
	}
	void Start(){
		tortureUI = TortureUI.Instace;
	}

	public void InitiateTorture () {
		GetGameManagerInstances ();
		LoadTortureData ();
		tortureUI.CreateTortureButtons ();
	}
	private void GetGameManagerInstances(){
		prisoners = GameManager.prisoners;
	}
	private void LoadTortureData(){
		TextAsset textDataFile = Resources.Load<TextAsset>("Texts/"+"torture_texts");
		string textDataString = textDataFile.text;
		string[] textDataLines = textDataString.Split (new string[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries);
		tortures = new Dictionary<string, Torture> ();
		foreach(string line in textDataLines) {
			string[] textEntry = line.Split ('\t');
			Torture torture = new Torture (textEntry);
			tortures [torture.name] = torture;
		}
	}
}
