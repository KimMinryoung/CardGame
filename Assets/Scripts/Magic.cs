using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic{
	public static Dictionary<string, Magic> magics;
	public string name;
	private static Action<Unit> NullConsumeMP = (unit) => {};
	Action<Unit> ConsumeMP = NullConsumeMP;
	private static Func<Unit, Unit, int> NullGetDamage = (caster, target) => { return 0; };
	Func<Unit, Unit, int> GetDamage = NullGetDamage;
	private static Action<Unit, int> NullDoTheDamage = (target, damage) => {};
	Action<Unit, int> DoTheDamage = NullDoTheDamage;
	List<Action> AllDoBuffs = new List<Action> ();

	public static void CreateAllMagics(){
		magics = new Dictionary<string, Magic> ();
		magics["평타"] = new Magic("평타");
		magics ["평타"].SetGetDamage (2.0);
		magics["강타"] = new Magic("강타");
		magics ["강타"].SetGetDamage(4.0);
	}

	public Magic(string name){
		this.name = name;
		SetDoTheDamage ();
	}
	private void SetMPConsumption(int MPconsumption){
		if (MPconsumption != 0) {
			ConsumeMP = (Unit caster) => {
				Dictionary<string, int> MPChange = new Dictionary<string, int> ();
				MPChange ["MP"] = -MPconsumption;
				caster.ChangeStatsAndAddMessages (MPChange);
			};
		}
	}
	private void SetGetDamage(double powerFactor){
		GetDamage = (Unit caster, Unit target) => {
			double power = powerFactor * caster.GetStat ("마법력");
			int damage = (int)Util.Max (1, power - target.GetStat ("저항력"));
			return damage;
		};
	}
	private void SetDoTheDamage(){
		DoTheDamage = (Unit target, int damage) => {
			Dictionary<string, int> changeStats = new Dictionary<string, int> ();
			changeStats.Add ("HP", -damage);
			target.ChangeStatsAndAddMessages (changeStats);
		};
	}
	public void Cast(Unit caster, Unit target){
		ConsumeMP (caster);
		int damage = GetDamage (caster, target);
		DoTheDamage (target, damage);
	}
}