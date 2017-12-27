using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Torture {
	public string name;
	private Dictionary<string,int> statChanges;
	public Action<Person> tortureAction;

	public Torture(string[] textEntry){
		name=textEntry[0];
		statChanges=new Dictionary<string,int>();
		int i;
		for(i=1;i<textEntry.Length;i+=2){
			statChanges.Add(textEntry[i],Convert.ToInt32(textEntry[i+1]));
		}
		tortureAction = (Person subject) => {
			if (Dialogue.dm.DuringDialogue())
				return;
			Func<string,string> Replace = (origin)=>{ return origin.Replace("{이름}",subject.GetName()); };
			//string fileName, string label, Func<string,string> ReplaceWords, Dictionary<string,int> comparedVariables
			Dialogue.dm.LoadDialogueFile("torture_dialogue_texts", name, Replace, subject.GetStats());
			subject.ChangeStatsAndAddMessages (statChanges);
		};
	}
}
