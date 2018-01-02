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

	Image background;
	Transform illustObject;
	Image portrait;
	Image nameBox;
	Text nameText;
	Image textBox;
	Text textText;

	Sprite transparentSprite;

	void Awake () {
		instance = this;

		Dialogue.dd = this;
		DialogueManager.dd = this;

		if (!manager)
			manager=GameObject.Find ("DialogueManager").GetComponent<Transform>();
		
		if (!background)
			background=manager.Find ("Background").GetComponent<Image>();
		if (!illustObject)
			illustObject=manager.Find ("Illust");
		if (!portrait)
			portrait=manager.Find ("Portrait").GetComponent<Image>();
		if (!nameBox)
			nameBox=manager.Find ("NameBox").GetComponent<Image>();
		if (!textBox)
			textBox=manager.Find ("TextBox").GetComponent<Image>();

		transparentSprite = Resources.Load<Sprite> ("UIImages/transparent");

		if (!nameText)
			nameText=manager.Find ("NameText").GetComponent<Text>();
		if (!textText)
			textText=manager.Find ("TextText").GetComponent<Text>();
		
	}
	void Start(){
		DialogueDisplayClear ();
	}
	public void DialogueDisplayClear(){
		RemovePortraitSprite ();
		RemoveIllustSprite ();
		DisableNameBox ();
		DisableTextBox ();
		PutNameText (null);
		PutTextText (null);
	}
	public void DisableNameBox(){
		nameBox.enabled = false;
	}
	public void EnableNameBox(){
		nameBox.enabled = true;
	}
	public void DisableTextBox(){
		textBox.enabled = false;
	}
	public void EnableTextBox(){
		textBox.enabled = true;
	}
	public void PutNameText(string text){
		nameText.text = text;
	}
	public void PutTextText(string text){
		textText.text = text;
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
}
