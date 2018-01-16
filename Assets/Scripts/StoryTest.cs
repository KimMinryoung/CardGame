using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryTest : MonoBehaviour {
	public InputField inputField;
	void Update(){
		if (inputField.text.Length > 0 && Input.GetKeyUp (KeyCode.Return) || Input.GetKeyUp (KeyCode.KeypadEnter)) {
			LoadSceneByInput ();
			Destroy (this.gameObject);
		}
	}
	public void LoadSceneByInput(){
		GameObject.Find ("DialogueManager").GetComponent<DialogueManager> ().LoadDialogueBySceneNumber (Convert.ToInt32 (inputField.text));
	}
}
