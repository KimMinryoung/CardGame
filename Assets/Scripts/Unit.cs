using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Unit : Person{
	Person person;
	public UnitUI unitUI;
	public enum Sides { Ally, Enemy };
	Sides side;
	public enum Activation { Behaveable, AlreadyBehaved, Deactivated };
	Activation activation;
	List<Buff> buffs;

	public Unit(Person person){
		this.person = person;
		this.name = person.GetName();
		AddStat ("HP", 10 * person.GetStat ("체력"), 0, 10 * person.GetStat ("체력"));
		AddStat ("MP", 10 * person.GetStat ("마력"), 0, 10 * person.GetStat ("마력"));
		AddStat ("마법력", 1000, 1, person.GetStat ("마력"));
		AddStat ("마법기술", 1000, 1, person.GetStat ("지능"));
		AddStat ("저항력", 1000, 1, person.GetStat ("정신"));
		AddStat ("순발력", 1000, 1, person.GetStat ("민첩"));
		AddStat ("운", 100, 1, person.GetStat ("행운"));

		magics = person.GetMagics();
		buffs = new List<Buff> ();
	}

	//아래 함수 개씹구리다 언젠간 고치길
	new public int GetStat(string name){
		double realValue = stats[name];
		foreach (Buff buff in buffs) {
			realValue = buff.ApplyBuff (name, realValue);
		}
		return GetStatValueInLimit(name, realValue);
	}
	public void CastMagic(Magic magic, Unit target){
		magic.Cast (this, target);
	}
	public Sides GetSide(){
		return side;
	}
	public bool IsAlly(){
		return side == Sides.Ally;
	}
	public bool IsEnemy(){
		return side == Sides.Enemy;
	}
	public void SetSide(Sides side){
		this.side = side;
	}
	public Activation GetActivation(){
		return activation;
	}
	public bool IsBehaveable(){
		return activation == Activation.Behaveable;
	}
	public void SetActivation(Activation activation){
		this.activation = activation;
	}
	new public string GetName(){
		return name;
	}
	new public List<Magic> GetMagics(){
		return magics;
	}
	public void AddBuff(Buff buff){
		buffs.Add (buff);
	}
	public void CreateHPAndMPStatBars (){
		List<string> statNames = new List<string> ();
		statNames.Add ("HP");
		statNames.Add ("MP");
		unitUI.CreateStatBars (statNames);
	}
	new public void ChangeStatsAndAddMessages(Dictionary<string, int> statChangesList){
		List<string> messages=new List<string>();
		string message;
		foreach(var statChange in statChangesList){
			message = ChangeStatAndGetMessage (statChange.Key, statChange.Value);
			if (message != null)
				messages.Add (message);
		}
		LoadDoubleMessages (messages);
	}
	private string ChangeStatAndGetMessage(string targetStat, int change){
		int prevStat = stats [targetStat];
		ChangeStatAndUpdateStatBar (targetStat, change);
		int realChange = stats [targetStat] - prevStat;
		string message = Util.AValueOfSomethingChangedMessage (realChange, targetStat, name);
		return message;
	}
	private void ChangeStatAndUpdateStatBar(string targetStat, int change){
		int prevStat = stats [targetStat];
		ChangeStat (targetStat, change);
		unitUI.UpdateStatBar (targetStat, prevStat);
	}
	public void Die(){
		unitUI.DestroyAll ();
	}
}
