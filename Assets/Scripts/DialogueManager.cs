﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {
	static DialogueManager instance;
	public static DialogueManager Instance {
		get { return instance; }
	}

	public static DialogueDisplay dd;

	public List<Dialogue> dialogues;
	public int lineNum;

	void Awake (){
		instance = this;
		Dialogue.dm = this;
		Person.dm = this;

		dialogues = new List<Dialogue>();
		lineNum = 0;
	}

	void Start(){
	}

	void Update(){
		if (SceneManager.GetActiveScene ().name == "Story") {
			if (!DuringDialogue ()) {
				LoadDialogueFile ("Scene#0", null, NoReplace, emptyCV);
			} else if (DuringDialogue ()) {
				if ((Input.GetKeyUp (KeyCode.Return)) || Input.GetKeyUp (KeyCode.Space)) {
					ClickForNextDialogueLine ();
				} else if (Input.GetKeyDown (KeyCode.L)) {
					OpenOrCloseLog ();
				}
			}
		}
	}

	public void ClickForNextDialogueLine(){
		if (dd.IsDialogueLogOpen ()) {
			OpenOrCloseLog ();
		} else {
			dd.AddDialogueLog ();
			ToNextLine ();
		}
	}

	public void OpenOrCloseLog(){
		if (dd.IsDialogueLogOpen ()) {
			dd.CloseDialogueLog ();
		} else {
			dd.OpenDialogueLog ();
		}
	}

	public void LoadDialogueFile(string fileName, string label, Func<string,string> ReplaceWords, Dictionary<string,int> comparedVariables){
		TextAsset dialogueTextAsset = Resources.Load<TextAsset> ("Texts/" + fileName);
		Debug.Log (fileName);
		Debug.Assert (dialogueTextAsset != null);
		string entireFile = dialogueTextAsset.text;
		string withinLabels = GetTextWithinLabels (label, entireFile);
		string dialoguesString = ReplaceWords (withinLabels);
		LoadDialoguesString (dialoguesString, comparedVariables);
	}
	string GetTextWithinLabels(string label, string entireText){
		if (label == null)
			return entireText;
		string codedLabel = "{" + label + "}";
		string[] parts = entireText.Split (new string[] { codedLabel }, StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length < 3) {
			Debug.Log ("다이얼로그 파일 로드 중 label '{"+label+"}'로 싸인 부분이 없어서 오류");
			return null;
		}
		string withinLabels;
		withinLabels = parts [1];
		return withinLabels;
	}
	void LoadDialoguesString(string dialoguesString, Dictionary<string,int> comparedVariables){
		string[] dialogueLines = dialoguesString.Split (new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		foreach(string line in dialogueLines) {
			LoadDialogueLine (line, comparedVariables);
		}
	}

	public void LoadDialogueLine(string line, Dictionary<string,int> comparedVariables){
		Dialogue dialogue = new Dialogue ();
		dialogue.LoadDialogueLine (line, comparedVariables);
		dialogues.Add (dialogue);
		if(lineNum == 0)
			ExecutePresentLine();
	}
	public void LoadMessageLine(string line){
		Dialogue dialogue = new Dialogue ();
		dialogue.LoadMessageLine (line);
		dialogues.Add (dialogue);
		if(lineNum == 0)
			ExecutePresentLine();
	}

	void DialoguesClear(){
		dialogues.Clear ();
		lineNum = 0;
	}

	bool LineOver(){
		if (lineNum >= dialogues.Count) {
			DialoguesClear ();
			dd.DialogueDisplayClear ();
			return true;
		}
		else
			return false;
	}
	public void ToNextLine(){
		lineNum++;
		if (LineOver ())
			return;
		ExecutePresentLine ();
	}
	public void ExecutePresentLine(){
		if (LineOver ())
			return;
		dialogues [lineNum].ExecuteDialogue ();
	}

	public bool DuringDialogue(){
		return dialogues.Count != 0;
	}
		
	public static Dictionary<string, int> emptyCV=new Dictionary<string,int>();
	public static Func<string, string> NoReplace = (a => a);
}
