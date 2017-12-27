using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff {
	int remainLife;
	string targetStat;
	public double factor;
	public Func<string, double, double> ApplyBuff = NullApplyBuff;
	private static Func<string, double, double> NullApplyBuff = (statName, statValue) => (statValue) ;
	public Buff(int life, string targetStat){
		this.remainLife = life;
		this.targetStat = targetStat;
		if (targetStat != null) {
			ApplyBuff = (string statName, double statValue) => {
				if (this.targetStat == statName)
					return factor * statValue;
				else
					return statValue;
			};
		}
	}
}
