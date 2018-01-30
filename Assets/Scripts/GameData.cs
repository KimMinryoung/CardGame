using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData{
	public static bool gameStarted = false;
	public static Dictionary<string, int> stats;

	public static void InitializeStats(){
		stats = new Dictionary<string, int> ();
		stats.Add ("이타심", 0);
		stats.Add ("자부심", 0);
	}
}

public class SaveData{
	public string sceneName;
	public Dictionary<string, int> stats;
	public void SetCurrentData(){
		sceneName = DialogueManager.currentDialogueSceneName;
		stats = GameData.stats;
	}
	public string GetDataString(){
		string data;
		data = sceneName + "\t";
		data += stats.Count + "\t";
		foreach (KeyValuePair<string, int> stat in stats) {
			data += stat.Key + "\t" + stat.Value + "\t";
		}
		return data;
	}
	public void LoadDataFromString(string data){
		string[] parts = data.Split('\t');
		int i = 0;
		sceneName = parts [i++];
		stats = new Dictionary<string, int> ();
		int statNum = Convert.ToInt32 (parts [i++]);
		for (int n = 0; n < statNum; n++) {
			stats.Add (parts [i++], Convert.ToInt32 (parts [i++]));
		}
	}
}