using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class UnitUI : MonoBehaviour
{
	Unit unit;
	private GameObject unitButton;
	Dictionary<string, Slider> statBars;
	private static GameObject StatBar;

	void Awake(){
		StatBar = GameManager.Instance.StatBar;
		statBars = new Dictionary<string, Slider> ();
	}

	public void SetUnit(Unit unit){
		this.unit = unit;
	}
	public void SetUnitButton(GameObject unitButton){
		this.unitButton = unitButton;
	}
	public GameObject GetUnitButton(){
		return unitButton;
	}
	public void CreateStatBars (List<string> statNames) {
		//DestroyStatBars ();
		int y = 100;
		foreach(string statName in statNames){
			CreateStatBar (statName, y);
			y -= 30;
		}
	}
	private void CreateStatBar(string name,  int y){
		GameObject statBarObject = MonoBehaviour.Instantiate (StatBar,unitButton.transform);
		statBarObject.transform.Translate(new Vector3 (0, y, 0));
		Slider statBar = statBarObject.GetComponent<Slider>();
		statBars[name] = statBar;
		UpdateStatBar (name, 0);
		statBarObject.transform.Find ("Text").GetComponent<Text>().text = name;
	}

	public void UpdateStatBar(string name, int prevStat){
		if(statBars.ContainsKey(name))
			StartCoroutine(SetStatBarValue (statBars [name], name, prevStat));
	}
	protected IEnumerator SetStatBarValue(Slider statBar, string name, int prevValue){
		statBar.maxValue = unit.GetStatMaxLimits() [name];
		int destinyValue = unit.GetStats() [name];
		int stepNum = 10;
		for (int i = 1; i <= stepNum; i++) {
			yield return new WaitForSeconds (0.05f);
			statBar.value = ((stepNum - i) * prevValue + i * destinyValue) / stepNum;
		}
	}
	public void DestroyAll(){
		foreach (var statBar in statBars) {
			Destroy (statBar.Value.gameObject);
		}
		Destroy (this.gameObject);
	}
}