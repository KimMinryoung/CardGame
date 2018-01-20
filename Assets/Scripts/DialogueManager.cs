using System;
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

	int frameWait = 0;
	public int fastDialogueFrameLag = 5;
	bool duringSkip = false;
	void Update(){
		if (DuringDialogue ()) {
			if ((Input.GetKeyUp (KeyCode.Return)) || Input.GetKey (KeyCode.KeypadEnter) || Input.GetKeyUp (KeyCode.Space)) {
				ClickForNextDialogueLine ();
			} else if (Input.GetKey (KeyCode.LeftControl) || duringSkip) {
				frameWait++;
				if (frameWait >= fastDialogueFrameLag) {
					frameWait = 0;
					ClickForNextDialogueLine ();
				}
			} else if (Input.GetKeyDown (KeyCode.L)) {
				OpenOrCloseLog ();
			}
		} else{
			if (SceneManager.GetActiveScene ().name == "Story") {
				GameData.InitializeStats ();
				LoadDialogueBySceneNumber (0);
			}
		}
	}

	public void LoadDialogueBySceneNumber(int sceneNumber){
		LoadDialogueFile ("Scene#" + sceneNumber, null, NoReplace, GameData.stats);
	}
	public void TurnOnOrOffSkip(){
		if (duringChoice) {
			return;
		}
		duringSkip = !duringSkip;
		frameWait = 0;
		dd.UpdateSkipButtonText (duringSkip);
	}
	public void ForciblyTurnOffSkip(){
		duringSkip = false;
		frameWait = 0;
		dd.UpdateSkipButtonText (duringSkip);
	}

	public void AddLogAndGoToNextLine(){
		dd.AddDialogueLog ();
		ToNextLine ();
	}

	public void ClickForNextDialogueLine(){
		if (dd.IsDialogueLogOpen ()) {
			OpenOrCloseLog ();
		} else if (duringChoice) {
			// do nothing;
		} else {
			AddLogAndGoToNextLine ();
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

	public bool duringChoice = false;
	public int choiceNum;

	public static Dictionary<string, int> emptyCV = new Dictionary<string,int>();
	public static Func<string, string> NoReplace = (a => a);
}
