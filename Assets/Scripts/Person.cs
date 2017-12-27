using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Person{
	protected string name;
	protected Dictionary<string,int> stats;
	protected Dictionary<string,int> statMaxLimits;
	protected Dictionary<string,int> statMinLimits;
	protected List<Magic> magics;

	public static DialogueManager dm;

	public Person () {
		stats=new Dictionary<string,int>();
		statMaxLimits = new Dictionary<string, int> ();
		statMinLimits = new Dictionary<string, int> ();
		magics = new List<Magic> ();
	}
	public Person(string name) : this(){
		this.name = name;
		AddStat ("체력", 100, 1, 1);
		AddStat ("정신", 100, 1, 1);
		AddStat ("민첩", 100, 1, 1);
		AddStat ("마력", 100, 1, 1);
		AddStat ("지능", 100, 1, 1);
		AddStat ("행운", 100, 1, 1);
		AddStat ("혐오", 100, 0, 0);
		AddStat ("복종", 100, 0, 0);
		AddStat ("공포", 100, 0, 0);
		AddStat ("광기", 100, 0, 0);
		AddStat ("애정", 100, 0, 0);
	}
	public Person(string name,int a,int b,int c, int d,int e,int f) : this(name){
		stats ["체력"] = a;
		stats ["정신"] = b;
		stats ["민첩"] = c;
		stats ["마력"] = d;
		stats ["지능"] = e;
		stats ["행운"] = f;
	}

	protected void AddStat(string statName, int max, int min){
		stats [statName] = min;
		statMaxLimits [statName] = max;
		statMinLimits [statName] = min;
	}
	protected void AddStat(string statName, int max, int min, int value){
		stats [statName] = value;
		statMaxLimits [statName] = max;
		statMinLimits [statName] = min;
	}

	public string GetName(){
		return name;
	}
	public int GetStat (string name){
		return stats [name];
	}
	public Dictionary<string, int> GetStats(){
		return stats;
	}
	public Dictionary<string, int> GetStatMaxLimits(){
		return statMaxLimits;
	}
	public List<Magic> GetMagics(){
		return magics;
	}
	public void AddMagic(string magicName){
		magics.Add (Magic.magics[magicName]);
	}
	public Sprite GetSprite(){
		Sprite sprite=Resources.Load<Sprite>("Portraits/"+name);
		return sprite;
	}

	protected void ChangeStat(string targetStat, int change){
		stats [targetStat] += change;
		stats [targetStat] = GetStatValueInLimit (targetStat, stats [targetStat]);
	}
	private string ChangeStatAndGetMessage(string targetStat, int change){
		int prevStat = stats [targetStat];
		ChangeStat (targetStat, change);
		int realChange = stats [targetStat] - prevStat;
		string message = Util.AValueOfSomethingChangedMessage (realChange, targetStat, name);
		return message;
	}
	public void ChangeStatsWithoutMessages(Dictionary<string, int> statChangesList){
		List<string> messages=new List<string>();
		foreach(var statChange in statChangesList){
			ChangeStat (statChange.Key, statChange.Value);
		}
	}
	public void ChangeStatsAndAddMessages(Dictionary<string, int> statChangesList){
		List<string> messages=new List<string>();
		string message;
		foreach(var statChange in statChangesList){
			message = ChangeStatAndGetMessage (statChange.Key, statChange.Value);
			if (message != null)
				messages.Add (message);
		}
		LoadDoubleMessages (messages);
	}
	protected void LoadDoubleMessages(List<string> messages){
		string doubleMessage;
		int i;
		for(i = 0 ; i + 1 < messages.Count ; i += 2){
			doubleMessage = messages [i] + "\n" + messages [i + 1];
			dm.LoadMessageLine (doubleMessage);
		}
		if (i == messages.Count - 1) {
			dm.LoadMessageLine (messages [i]);
		}
	}

	protected int GetStatValueInLimit(string statName, double value){
		double realValue = value;
		realValue = (int)Util.Max (realValue, statMinLimits [statName]);
		realValue = (int)Util.Min (realValue, statMaxLimits [statName]);
		return (int)realValue;
	}
}
