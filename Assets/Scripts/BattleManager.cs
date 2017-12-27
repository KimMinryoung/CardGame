using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {
	public static BattleManager instance;
	public static BattleManager Instance {
		get { return instance; }
	}
	public static List<Unit> units;

	public static GameObject Canvas;
	private static GameObject ImageButton;
	private static GameObject SmallButton;

	enum State { Base, ClickedUnit, ClickedBehaviour };
	static State state;
	static Unit selectedUnit = null;
	static Magic selectedBehaviour = null;

	void Awake(){
		instance = this;
	}

	public void InitiateBattle (){
		GetGameManagerInstances ();

		state = State.Base;
		units = new List<Unit> ();
		List<string> allyUnits = new List<string> ();
		List<string> enemyUnits = new List<string> ();
		allyUnits.Add ("젠");
		allyUnits.Add ("아이리스");
		LoadUnitsOfSides (allyUnits, Unit.Sides.Ally);
		enemyUnits.Add ("밀피");
		LoadUnitsOfSides (enemyUnits, Unit.Sides.Enemy);
		PutBackground ();
		CreateUnitButtons ();

		StartAllyTurn ();
	}
	void GetGameManagerInstances(){
		ImageButton = GameManager.Instance.ImageButton;
		SmallButton = GameManager.Instance.SmallButton;
	}
	void LoadUnitsOfSides(List<string> unitNames, Unit.Sides side){
		foreach (string name in unitNames) {
			Unit unit = new Unit (GameManager.persons [name]);
			units.Add (unit);
			unit.SetSide (side);
		}
	}
	void PutBackground(){
		DialogueDisplay.Instance.PutBackgroundSprite ("Blue");
	}
	void CreateUnitButtons(){
		int x, y;
		int ySpace = 200;

		List<Unit> allyUnits = GetAllyUnits ();
		int allyUnitsNum = allyUnits.Count();
		x = -500;
		y = 250;
		foreach (Unit unit in allyUnits) {
			CreateUnitButton (unit, x, y);
			y -= ySpace;
		}

		List<Unit> enemyUnits = GetEnemyUnits ();
		int enemyUnitsNum = enemyUnits.Count();
		x = 500;
		y = 250;
		foreach (Unit unit in enemyUnits) {
			CreateUnitButton (unit, x, y);
			y -= ySpace;
		}
	}
	void CreateUnitButton(Unit unit, int x,int y){
		GameObject button;
		button = Util.CreateButton (ImageButton, Canvas.transform, x, y, null, () => { 
			if(state == State.Base){
				if(unit.IsAlly() && unit.IsBehaveable()){
					state = State.ClickedUnit;
					selectedUnit = unit;
					CreateBehaviourButtons(unit, x+230, y); 
				}
			}
			else if(state == State.ClickedBehaviour){
				state = State.Base;
				DestroyBehaviourButtons();
				selectedBehaviour.Cast(selectedUnit, unit);
				EndUnitBehave(selectedUnit);
			}
		} );
		button.AddComponent<UnitUI> ();
		unit.unitUI = button.GetComponent<UnitUI> ();
		unit.unitUI.SetUnit (unit);
		unit.unitUI.SetUnitButton (button);
		button.GetComponent<Image> ().sprite = unit.GetSprite ();
		unit.CreateHPAndMPStatBars ();
	}

	static List<GameObject> behaviourButtons;

	void CreateBehaviourButtons(Unit unit, int x,int y){
		behaviourButtons = new List<GameObject> ();
		int ySpace = 50;
		foreach(Magic magic in unit.GetMagics()) {
			GameObject button;
			button = Util.CreateButton (SmallButton, Canvas.transform, x, y, magic.name, () => {
				state = State.ClickedBehaviour;
				selectedBehaviour = magic;
			});
			behaviourButtons.Add (button);
			y -= ySpace;
		}
	}
	void DestroyBehaviourButtons(){
		foreach (var button in behaviourButtons) {
			MonoBehaviour.Destroy (button);
		}
	}

	void StartAllyTurn(){
		List<Unit> allyUnits = GetAllyUnits ();
		foreach (var unit in allyUnits) {
			LetUnitBehave (unit);
		}
	}
	bool CheckNoAllyBehaveLeft(){
		bool noAllyLeft = true;
		List<Unit> allyUnits = GetAllyUnits ();
		foreach (var unit in allyUnits) {
			if (unit.IsBehaveable())
				noAllyLeft = false;
		}
		return noAllyLeft;
	}
	void EndAllyTurn(){
		StartAllyTurn ();
	}

	void LetUnitBehave(Unit unit){
		unit.SetActivation (Unit.Activation.Behaveable);
	}
	void EndUnitBehave(Unit unit){
		unit.SetActivation (Unit.Activation.AlreadyBehaved);
		KillZeroHPUnits ();
		if (CheckBattleEndAndExecuteEnding ()) {
			return;
		}
		CheckTurnEnd ();
	}
	void KillZeroHPUnits(){
		for (int i=units.Count-1;i>=0;i--){
			Unit unit = units [i];
			if (unit.GetStat ("HP") == 0) {
				unit.Die ();
				units.Remove (unit);
			}
		}
	}
	bool CheckBattleEndAndExecuteEnding(){
		if (NoEnemyLeft ()) {
			WinBattle ();
			return true;
		}
		if (NoAllyLeft ()) {
			LoseBattle ();
			return true;
		}
		return false;
	}
	void CheckTurnEnd(){
		if (CheckNoAllyBehaveLeft ()) {
			EndAllyTurn ();
		}
	}
	void DeactivateUnitBehave(Unit unit){
		unit.SetActivation (Unit.Activation.Deactivated);
		//unit.unitUI.GetUnitButton ().GetComponent<Button> ().interactable = false;
	}
	void WinBattle(){
		DialogueManager.Instance.LoadMessageLine ("전투 승리");
		// win battle!!!!!!!!
	}
	void LoseBattle(){
		DialogueManager.Instance.LoadMessageLine ("전투 패배");
		// lose battle!!!!!!!!
	}
	List<Unit> GetAllyUnits(){
		IEnumerable<Unit> allyUnits =
			from unit in units
				where unit.IsAlly()
			select unit;
		return allyUnits.ToList();
	}
	List<Unit> GetEnemyUnits(){
		IEnumerable<Unit> enemyUnits =
			from unit in units
				where unit.IsEnemy()
			select unit;
		return enemyUnits.ToList();
	}
	bool NoAllyLeft(){
		return GetAllyUnits().Count ==  0;
	}
	bool NoEnemyLeft(){
		return GetEnemyUnits().Count ==  0;
	}
}
