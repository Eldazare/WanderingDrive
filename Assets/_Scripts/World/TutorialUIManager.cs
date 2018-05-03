using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIManager : MonoBehaviour {

	public GameObject tutorialInfoBoxObject;
	private Text tutorialInfoBox;
	private string theText;

	private bool tutorialActive;

	void Start(){
		DontDestroyOnLoad (this.gameObject);
		tutorialActive = true;
		tutorialInfoBox = tutorialInfoBoxObject.GetComponent<Text> ();
	}

	void Update(){
		if (tutorialActive) {
			tutorialInfoBox.text = this.theText;
		} else {
			tutorialInfoBoxObject.SetActive (false);
		}
	}

	public void ModifyText(string theText){
		this.theText = theText;
	}

	public void ExitTutorial(){
		Destroy (this.gameObject);
	}

	public void SetEquipmentExplanation(string type){
		switch (type) {
		case "weapon":
			theText = "Weapons & Damage:\n" +
			"Weapons have different elements and damage type. Craft and choose correct one!\n" +
			"In addition to damage, weapons affect your block / dodge window.";
			break;
		case "armor":
			break;
		case "accessory":
			break;
		}
	}

	public void SetCombatExplanation(){
	
	}
}
