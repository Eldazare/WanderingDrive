using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {


	public CombatController combatController;
	public Enemy targetedEnemy;


	public GameObject DefaultButtons, AbilityButtons, ItemMenu, textBox;
	public Button focusButton, overloadButton; //Drag focus and overload buttons to menuController
	public PlayerCombatScript player;
	public bool focusEnabled, overloadEnabled;
	int enemyTargetNumber;
	public Image playerHealthFill, playerManaFill;
	public Text playerHealthText, playerManaText, textBoxText;
	public Image[] enemyHealthFills;
	public Text[] enemyHealthTexts;
	public GameObject[] enemyHealthBars;
	public GameObject playerPovCamera;
	public float textSpeed;
	public bool proceed;

	


	void Start(){
		focusEnabled = true;
		overloadEnabled = true;
	}	

	//Doesn't actually do anything yet
	//Uses the linked list concept, to switch targets
	public void NextTarget () {
		enemyTargetNumber++;
		if(enemyTargetNumber > combatController.enemyList.Count){
			enemyTargetNumber = 0;
		}
		targetedEnemy = combatController.enemyList[enemyTargetNumber];
	}
	public void PreviousTarget() {
		enemyTargetNumber--;
		if(enemyTargetNumber < 0){
			enemyTargetNumber = combatController.enemyList.Count;
		}
		targetedEnemy = combatController.enemyList[enemyTargetNumber];
	}

 	//Buttons for button menus.

	public void AbilitiesMenu() {
		DefaultButtons.SetActive (false);
		AbilityButtons.SetActive (true);
		focusButton.interactable = focusEnabled;
		overloadButton.interactable = overloadEnabled;
	}
	public void ItemsMenu () {
		ItemMenu.SetActive (true);
		DefaultButtons.SetActive (false);
	}
	public void Back () {
		DefaultButtons.SetActive (true);
		ItemMenu.SetActive (false);
		AbilityButtons.SetActive (false);
	}
	public void PlayersTurn(){
		StartCoroutine(AttackWaitTime());
	}

	// Buttons.
	public void Attack () {
		DefaultButtons.SetActive(false);
		//player.Attack ();
		combatController.cameraScript.MoveCamera(targetedEnemy.cameraTarget);
	}
	public void ChoosePartToAttack(int part){
		//player.Attack(part);
		StartCoroutine(PlayerAttack(part));
	}
	IEnumerator PlayerAttack(int part){
		proceed = false;
		combatController.cameraScript.MoveCamera(playerPovCamera);
		yield return new WaitUntil(()=>proceed);
		combatController.cameraScript.FollowTarget(playerPovCamera);
		player.Attack(part);
	}
	public void Ability(int slot) {
		AbilityButtons.SetActive (false);
		player.Ability (slot);
	}

	public void Consumable(int slot){
		ItemMenu.SetActive (false);
		player.Consumable(slot);
	}
	public void Focus() {
		AbilityButtons.SetActive (false);
		player.PlayerFocus ();
	}
	public void Overload () {
		AbilityButtons.SetActive (false);
		player.PlayerOverload ();
	}

	//UI updates
	public void updatePlayerHealth(float health, float maxHealth, float percentage){
		playerHealthFill.fillAmount = percentage;
		playerHealthText.text = health.ToString() + "/" + maxHealth.ToString();
	}
	public void updateEnemyHealth(float health, float maxHealth, float percentage, Enemy enemyForListSearch){
		enemyHealthFills[combatController.enemyList.IndexOf(enemyForListSearch)].fillAmount = percentage;
		enemyHealthTexts[combatController.enemyList.IndexOf(enemyForListSearch)].text = health.ToString() +"/"+maxHealth.ToString();
	}

	public void messageToScreen(string message){
		textBox.SetActive(true);
		StartCoroutine(writeText(message));
	}
	IEnumerator writeText(string message){
		foreach (var item in message)
		{
			textBoxText.text += item;
			yield return new WaitForSeconds(textSpeed);
		}
		yield return new WaitForSeconds(1.5f);
		textBoxText.text = "";
		textBox.SetActive(false);
	}

	IEnumerator AttackWaitTime(){
		yield return new WaitForSeconds(1.5f);
		DefaultButtons.SetActive(true);
		textBoxText.text = "";
		textBox.SetActive(false);
	}

}
