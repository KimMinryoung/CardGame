using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

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

	public static SaveData NewGameData(){
		SaveData newGameData = new SaveData ();
		newGameData.sceneName = "Scene#0";
		GameData.InitializeStats ();
		newGameData.stats = new Dictionary<string, int> (GameData.stats);
		return newGameData;
	}
	public void SetCurrentData(){
		sceneName = DialogueManager.currentDialogueSceneName;
		stats = new Dictionary<string,int> (GameData.stats);
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

	public static void SaveCurrentStatusToFile(){	
		SaveData saveData = new SaveData ();
		saveData.SetCurrentData ();
		SaveDataToFile (saveData);
	}
	public static void SaveDataToFile(SaveData saveData){	
		string stringData = saveData.GetDataString ();
		string filePath = Application.persistentDataPath + "/save.csv";
		File.WriteAllText(filePath, stringData, Encoding.UTF8);
	}

	public static SaveData LoadFromFile(){
		string filePath = Application.persistentDataPath + "/save.csv";
		if (!File.Exists (filePath)) {
			Debug.Log("Save file does not exist. Made a new save file at " + filePath);
			SaveData newGameData = SaveData.NewGameData ();
			SaveDataToFile (newGameData);
		}
		string stringData = File.ReadAllText(filePath, Encoding.UTF8);
		return LoadDataFromString (stringData);
	}
	static SaveData LoadDataFromString(string data){
		SaveData loadedData = new SaveData ();
		string[] parts = data.Split('\t');
		int i = 0;
		loadedData.sceneName = parts [i++];
		loadedData.stats = new Dictionary<string, int> ();
		int statNum = Convert.ToInt32 (parts [i++]);
		for (int n = 0; n < statNum; n++) {
			loadedData.stats.Add (parts [i++], Convert.ToInt32 (parts [i++]));
		}
		return loadedData;
	}
}