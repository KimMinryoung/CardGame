using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour {
	static DialogueDisplay instance;
	public static DialogueDisplay Instance {
		get { return instance; }
	}

	public Transform manager;
	DialogueManager dm;

	Image background;
	Transform illustObject;
	Image portrait;
	Image nameBox;
	Text nameText;
	Image contentBox;
	Text contentText;

	public GameObject SmallButton;
	public List<GameObject> choiceButtons;
	Transform choiceButtonsContainer;

	GameObject dialogueLogScrollView;
	GameObject dialogueLogBox;
	Text dialogueLogContentText;
	Text dialogueLogNameText;

	Sprite transparentSprite;

	void Awake () {
		instance = this;

		Dialogue.dd = this;
		DialogueManager.dd = this;

		manager=GameObject.Find ("DialogueManager").GetComponent<Transform>();
		dm = manager.GetComponent<DialogueManager> ();
		
		background=manager.Find ("Background").GetComponent<Image>();
		illustObject=manager.Find ("Illust");
		portrait=manager.Find ("Portrait").GetComponent<Image>();
		nameBox=manager.Find ("NameBox").GetComponent<Image>();
		nameText=manager.Find ("NameBox").Find ("NameText").GetComponent<Text>();
		contentBox=manager.Find ("ContentBox").GetComponent<Image>();
		contentText=manager.Find ("ContentBox").Find ("ContentText").GetComponent<Text>();
		choiceButtonsContainer = manager.Find ("ChoiceButtonsContainer");
		dialogueLogScrollView = manager.Find ("DialogueLogScrollView").gameObject;
		dialogueLogBox = GameObject.Find ("DialogueLogBox").gameObject;
		dialogueLogNameText = dialogueLogBox.transform.Find ("DialogueLogNameText").GetComponent<Text>();
		dialogueLogContentText = dialogueLogBox.transform.Find ("DialogueLogContentText").GetComponent<Text> ();
		dialogueLogScrollView.SetActive (false);

		transparentSprite = Resources.Load<Sprite> ("UIImages/transparent");

		dialogueLogs = new List<DialogueLog> ();
	}
	void Start(){
		DialogueDisplayClear ();
	}
	public void DialogueDisplayClear(){
		RemovePortraitSprite ();
		RemoveIllustSprite ();
		DisableNameBox ();
		DisableContentBox ();
		PutNameText (null);
		PutContentText (null);
	}
	public void DisableNameBox(){
		nameBox.enabled = false;
	}
	public void EnableNameBox(){
		nameBox.enabled = true;
	}
	public void DisableContentBox(){
		contentBox.enabled = false;
	}
	public void EnableContentBox(){
		contentBox.enabled = true;
	}
	public void PutNameText(string text){
		nameText.text = text;
	}
	public void PutContentText(string text){
		contentText.text = text;
	}
	public void RemoveBackgroundSprite(){
		PutBackgroundSprite (transparentSprite);
	}
	void PutBackgroundSprite(Sprite sprite){
		background.sprite = sprite;
	}
	public void PutBackgroundSprite(String name){
		Sprite sprite=Resources.Load<Sprite>("Backgrounds/"+name);
		PutBackgroundSprite(sprite);
	}
	public void RemovePortraitSprite(){
		PutPortraitSprite (transparentSprite);
	}
	void PutPortraitSprite(Sprite sprite){
		portrait.sprite = sprite;
	}
	public void PutPortraitSprite(String name){
		Sprite sprite=Resources.Load<Sprite>("Portraits/"+name);
		if (sprite == null) {
			RemovePortraitSprite ();
		} else {
			PutPortraitSprite (sprite);
		}
	}
	public void BlackenPortraitSprite(){
		portrait.color = Color.black;
	}
	public void BrightenPortraitSprite(){
		portrait.color = Color.white;
	}
	public void RemoveIllustSprite(){
		PutIllustSprite (transparentSprite);
	}
	void PutIllustSprite(Sprite sprite){
		illustObject.GetComponent<Image>().sprite = sprite;
		illustObject.GetComponent<RectTransform> ().sizeDelta = sprite.rect.size;
	}
	public void PutIllustSprite(String name){
		Sprite sprite=Resources.Load<Sprite>("Illusts/"+name);
		PutIllustSprite(sprite);
	}
	bool isShaking = false;
	float remainShakePower = 0.0f;
	public void StartShaking(float initialPower){
		isShaking = true;
		remainShakePower = initialPower;
	}

	//  Choices showing part starts
	public void CreateChoiceButtons(List<string> choices){
		int x = 0;
		int y = 150;
		int ySpace = 75;
		choiceButtons = new List<GameObject> ();
		for (int i = 0; i < choices.Count; i++) {
			GameObject button;
			int index = i;
			button = Util.CreateButton (SmallButton, choiceButtonsContainer, x, y, choices [index], () => {
				dm.choiceNum = index + 1;
				DestroyChoiceButtons();
				dm.ClickForNextDialogueLine();
			});
			choiceButtons.Add (button);
			y -= ySpace;
		}
	}
	void DestroyChoiceButtons(){
		for (int i = choiceButtons.Count - 1; i >= 0; i--) {
			GameObject button = choiceButtons [i];
			choiceButtons.Remove (button);
			GameObject.Destroy (button);
		}
	}
	//  Choices showing part ends

	// Dialogue log part starts
	public class DialogueLog{
		public string speakerName;
		public string content;
		public DialogueLog(string speakerName, string content){
			this.speakerName = speakerName;
			this.content = content;
		}
	}
	public static List<DialogueLog> dialogueLogs = new List<DialogueLog> ();
	public void AddDialogueLog(){
		dialogueLogs.Add (new DialogueLog (nameText.text, contentText.text.Replace("\n"," ")));
	}
	void PrintDialogueLog(){
		foreach (DialogueLog log in dialogueLogs) {
			dialogueLogNameText.text += "\n" + log.speakerName;
			dialogueLogContentText.text += "\n" + log.content;
		}
	}
	public void OpenDialogueLog(){
		dialogueLogNameText.text = null;
		dialogueLogContentText.text = null;
		PrintDialogueLog ();
		RectTransform logBoxRect = dialogueLogBox.GetComponent<RectTransform> ();
		logBoxRect.sizeDelta = new Vector2 (logBoxRect.rect.width, Math.Max (500, dialogueLogNameText.preferredHeight + 30));

		dialogueLogScrollView.SetActive (true);
	}
	public void CloseDialogueLog(){
		dialogueLogScrollView.SetActive (false);
	}
	public bool IsDialogueLogOpen(){
		return dialogueLogScrollView.activeInHierarchy;
	}
	// Dialogue log part ends

	void Update () {
		if (isShaking) {
			if (remainShakePower > 0) {
				remainShakePower -= 1.0f * Time.deltaTime;
				Vector2 offset = 10 * UnityEngine.Random.insideUnitCircle * remainShakePower;
				gameObject.transform.localPosition = new Vector3 (offset.x, offset.y, gameObject.transform.localPosition.z);
			} else {
				gameObject.transform.localPosition = new Vector3 (0, 0, gameObject.transform.localPosition.z);
				remainShakePower = 0;
				isShaking = false;
			}
		}
	}
}
