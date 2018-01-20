using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryTest : MonoBehaviour {
	public InputField inputField;
	void Update(){
		if (Input.GetKeyUp (KeyCode.Return) || Input.GetKeyUp (KeyCode.KeypadEnter)) {
			if (inputField.text.Length > 0) {
				LoadSceneByInput ();
				Destroy (this.gameObject);
			}
		}
	}
	public void LoadSceneByInput(){
		GameData.InitializeStats ();
		GameObject.Find ("DialogueManager").GetComponent<DialogueManager> ().LoadDialogueBySceneNumber (Convert.ToInt32 (inputField.text));
	}
}
