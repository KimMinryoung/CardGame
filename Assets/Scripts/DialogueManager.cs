using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {
	private static DialogueManager instance;
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
			if (DuringDialogue () && (Input.GetMouseButtonDown (0) || Input.GetKeyDown (KeyCode.Return))) {
				ToNextLine ();
			} else if (!DuringDialogue ()) {
				LoadDialogueFile ("Scene#0", null, NoReplace, emptyCV);
			}
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
	private string GetTextWithinLabels(string label, string entireText){
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
	private void LoadDialoguesString(string dialoguesString, Dictionary<string,int> comparedVariables){
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

	private void DialoguesClear(){
		dialogues.Clear ();
		lineNum = 0;
	}

	private bool LineOver(){
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
