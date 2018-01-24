using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData{
	public static bool gameStarted = false;
	public static Dictionary<string, int> stats;

	public static void InitializeStats(){
		stats = new Dictionary<string, int> ();
		stats.Add ("이타심", 1);
		stats.Add ("자부심", 1);
		stats.Add ("연구", 1);
		stats.Add ("바올리 호감도", 1);
	}
}
